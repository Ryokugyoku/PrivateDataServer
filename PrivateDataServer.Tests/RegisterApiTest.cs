namespace PrivateDataServer.Tests;
using Microsoft.AspNetCore.Mvc.Testing;
using PrivateDataServer.Api.UserModule.Model;
using System.Text.Json;
using System.Net;
using Microsoft.EntityFrameworkCore;
using PrivateDataServer.DB;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

/// <summary>
/// アカウント作成APIテスト
/// </summary>
public class RegisterApiTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public RegisterApiTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // データベースコンテキストの設定をインメモリデータベースに変更
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });

                    // サービスプロバイダーを構築
                    var serviceProvider = services.BuildServiceProvider();

                    // データベースを初期化
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        dbContext.Database.EnsureCreated();
                    }
                });
            });
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