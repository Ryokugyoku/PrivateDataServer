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
using System.Threading;

/// <summary>
/// アカウント作成APIテスト
/// </summary>
public class RegisterApiTestFixture : IDisposable
{
    public WebApplicationFactory<Program> Factory { get; }
    public HttpClient Client { get; }
    public PostgreSqlContainer PostgresContainer { get; }

    public RegisterApiTestFixture()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
        Factory = new WebApplicationFactory<Program>();
        PostgresContainer = new PostgreSqlBuilder()
            .WithDatabase("autotest")
            .WithUsername("test")
            .WithPassword("test")
            .WithPortBinding(6543,5432) // この行を削除
            .WithHostname("localhost")
            .Build();

        // コンテナの開始
        PostgresContainer.StartAsync().GetAwaiter().GetResult();
        Console.WriteLine("PostgreSQL Container Connection String: " + PostgresContainer.GetConnectionString());

        // 接続文字列の取得
        var connectionString = PostgresContainer.GetConnectionString();

        Factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // PostgreSQL データベースプロバイダーを登録
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                });
            });
        });

        // アプリケーションの起動を待つ
        Client = Factory.CreateClient();
    }

    public void Dispose()
    {
        Client.Dispose();
        Factory.Dispose();
        PostgresContainer.DisposeAsync().GetAwaiter().GetResult();
    }
}

/// <summary>
/// アカウント作成APIテスト
/// </summary>
public class RegisterApiTest : IClassFixture<RegisterApiTestFixture>
{
    private readonly RegisterApiTestFixture _fixture;

    public RegisterApiTest(RegisterApiTestFixture fixture)
    {
        _fixture = fixture;
    }

    /// <summary>
    /// 正常入力値のテスト
    /// </summary>
    [Fact]
    public async Task InputColumnTest1()
    {
        var request = new CreateUserRequest
        {
            UserName = "TestUser",
            Email = "testuser@example.com",
            Password = "Test@123"
        };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _fixture.Client.PostAsync("/api/UserManager/CreateUser", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    /// <summary>
    /// Emailがnullのテスト
    /// </summary>
    [Fact]
    public async Task InputColumnTest2()
    {
        var request = new CreateUserRequest
        {
            UserName = "TestUser",
            Email = null,
            Password = "Test@123"
        };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _fixture.Client.PostAsync("/api/UserManager/CreateUser", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    /// <summary>
    /// Passwordがnullのテスト
    /// </summary>
    [Fact]
    public async Task InputColumnTest3()
    {
        var request = new CreateUserRequest
        {
            UserName = "TestUser",
            Email = "Test@mail.com",
            Password = null
        };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _fixture.Client.PostAsync("/api/UserManager/CreateUser", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    /// <summary>
    /// UserNameがnullのテスト
    /// </summary>
    [Fact]
    public async Task InputColumnTest4()
    {
        var request = new CreateUserRequest
        {
            UserName = null,
            Email = "Test@mail.com",
            Password = "Test@123"
        };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _fixture.Client.PostAsync("/api/UserManager/CreateUser", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    /// <summary>
    /// 全てがnullのテスト
    /// </summary>
    [Fact]
    public async Task InputColumnTest5()
    {
        var request = new CreateUserRequest
        {
            UserName = null,
            Email = null,
            Password = null
        };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _fixture.Client.PostAsync("/api/UserManager/CreateUser", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    /// <summary>
    /// Emailだけのテスト
    /// </summary>
    [Fact]
    public async Task InputColumnTest6()
    {
        var request = new CreateUserRequest
        {
            UserName = null,
            Email = "Test@test.com",
            Password = null
        };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _fixture.Client.PostAsync("/api/UserManager/CreateUser", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    /// <summary>
    /// UserNameだけのテスト
    /// </summary>
    [Fact]
    public async Task InputColumnTest7()
    {
        var request = new CreateUserRequest
        {
            UserName = "test",
            Email = null,
            Password = null
        };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _fixture.Client.PostAsync("/api/UserManager/CreateUser", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    /// <summary>
    /// Passwordだけのテスト
    /// </summary>
    [Fact]
    public async Task InputColumnTest8()
    {
        var request = new CreateUserRequest
        {
            UserName = null,
            Email = null,
            Password = "test"
        };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _fixture.Client.PostAsync("/api/UserManager/CreateUser", content);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}