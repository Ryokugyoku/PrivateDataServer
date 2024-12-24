using Serilog;
using Serilog.Sinks.PostgreSQL;
using NpgsqlTypes;
using PrivateDataServer.Module.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PrivateDataServer.Module.LogModule;
using PrivateDataServer.Module.UserModule.Interface;
namespace PrivateDataServer;
public class Program{
    public static void Main(string[] args){
        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen(); // Swaggerのサービスを追加

        // Serilog の設定読み込み
        builder.Host.UseSerilog((context, services, configuration) => {
            var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

            // カラムオプションの定義
            var columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                { "Timestamp", new TimestampColumnWriter() },
                { "Level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                { "Message", new RenderedMessageColumnWriter() },
                { "MessageTemplate", new MessageTemplateColumnWriter() },
                { "Exception", new ExceptionColumnWriter() },
                { "Properties", new LogEventSerializedColumnWriter() },
                { "ExecUser", new SinglePropertyColumnWriter("ExecUser", PropertyWriteMethod.Raw) }
            };

            configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.WithProperty("ExecUser", "SYSTEM")
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.PostgreSQL(
                    connectionString: connectionString,
                    tableName: "Logs",
                    needAutoCreateTable: true, 
                    columnOptions: columnWriters
                );
        });
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder.WithOrigins("http://localhost","https://localhost","http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        
        builder.Services.AddScoped<ILoginService, LoginService>();
        builder.Services.AddScoped<IUserManagerService, UserManagerService>();


        var app = builder.Build();
        // カスタムミドルウェアの追加
        app.UseMiddleware<ExecUserLoggingMiddleware>();
        app.UseCors();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //詳細なエラーが出るようにする。
            app.UseDeveloperExceptionPage();
            //Swaggerの設定
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PrivateDataServer v1");
                    c.RoutePrefix = string.Empty; // Swagger UIをルートに設定
                });
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}