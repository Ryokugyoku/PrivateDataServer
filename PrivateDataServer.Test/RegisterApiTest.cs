namespace PrivateDataServer.Test;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using PrivateDataServer.Module.DB;
using PrivateDataServer.Module.Request;
using Testcontainers.PostgreSql;

/// <summary>
/// アカウント作成APIテスト
/// </summary>
public class RegisterApiTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    private readonly PostgreSqlContainer _postgresContainer;

    public RegisterApiTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _postgresContainer = new PostgreSqlBuilder()
            .WithDatabase("mydatabase")
            .WithUsername("myusername")
            .WithPassword("mypassword")
            .WithPortBinding(5432)
            .WithHostname("localhost")
            .Build();

            // コンテナの開始
         _postgresContainer.StartAsync().GetAwaiter().GetResult();
        // コンテナが完全に起動するまで待機
        Task.Delay(5000).Wait();
        _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // PostgreSQL データベースプロバイダーを登録
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(_postgresContainer.GetConnectionString());
                });

                // サービスプロバイダーを構築
                var serviceProvider = services.BuildServiceProvider();

                // データベースを初期化
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    dbContext.Database.EnsureDeleted(); // 既存のデータベースを削除
                    dbContext.Database.EnsureCreated(); // 新しいデータベースを作成
                    dbContext.Database.Migrate();//マイグレーションを実施
                }
            });
        });

        // アプリケーションの起動を待つ
        _client = _factory.CreateClient();
    }
    
    /// <summary>
    /// 入出力テスト
    /// </summary>
    [Fact]
    public async Task InputColumnTest()
    {
        //正常入力値
        var request = new CreateUserRequest
        {
            UserName = "TestUser",
            Email = "testuser@example.com",
            Password = "Test@123"
        };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/UserManager/CreateUser", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        //Emailがnull
        request = new CreateUserRequest
        {
            UserName = "TestUser",
            Email = null,
            Password = "Test@123"
        };

        json = JsonSerializer.Serialize(request);
        content = new StringContent(json, Encoding.UTF8, "application/json");
        response = await _client.PostAsync("/api/UserManager/CreateUser", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        //Passwordがnull
        request = new CreateUserRequest
        {
            UserName = "TestUser",
            Email = "Test@mail.com",
            Password = null
        };

        json = JsonSerializer.Serialize(request);
        content = new StringContent(json, Encoding.UTF8, "application/json");
        response = await _client.PostAsync("/api/UserManager/CreateUser", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        //UserNameがnull
        request = new CreateUserRequest
        {
            UserName = null,
            Email = "Test@mail.com",
            Password = "Test@123"
        };

        json = JsonSerializer.Serialize(request);
        content = new StringContent(json, Encoding.UTF8, "application/json");
        response = await _client.PostAsync("/api/UserManager/CreateUser", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        //全てがnull
        request = new CreateUserRequest
        {
            UserName = null,
            Email = null,
            Password = null
        };

        json = JsonSerializer.Serialize(request);
        content = new StringContent(json, Encoding.UTF8, "application/json");
        response = await _client.PostAsync("/api/UserManager/CreateUser", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        //Emailだけのとき
        request = new CreateUserRequest
        {
            UserName = null,
            Email = "Test@test.com",
            Password = null
        };

        json = JsonSerializer.Serialize(request);
        content = new StringContent(json, Encoding.UTF8, "application/json");
        response = await _client.PostAsync("/api/UserManager/CreateUser", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //UserNameだけのとき
        request = new CreateUserRequest
        {
            UserName = "test",
            Email = null,
            Password = null
        };

        json = JsonSerializer.Serialize(request);
        content = new StringContent(json, Encoding.UTF8, "application/json");
        response = await _client.PostAsync("/api/UserManager/CreateUser", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        //passwordだけのとき
        request = new CreateUserRequest
        {
            UserName = null,
            Email = null,
            Password = "test"
        };

        json = JsonSerializer.Serialize(request);
        content = new StringContent(json, Encoding.UTF8, "application/json");
        response = await _client.PostAsync("/api/UserManager/CreateUser", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}