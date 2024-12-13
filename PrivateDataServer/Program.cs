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

        builder.Services.AddOpenApi();

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

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
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