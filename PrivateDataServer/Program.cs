using System.Reflection;
using PrivateDataServer.Api.LoginModule;
using PrivateDataServer.DB;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using NpgsqlTypes;
using PrivateDataServer.Api.UserModule.Services;
using PrivateDataServer.Middleware;
public class Program{

    public static void Main(string[] args){
        var builder = WebApplication.CreateBuilder(args);

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

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddScoped<ILoginService, LoginService>();
        builder.Services.AddScoped<IUserManagerService, UserManagerService>();
        builder.Services.AddControllersWithViews();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        var app = builder.Build();
        // カスタムミドルウェアの追加
        app.UseMiddleware<ExecUserLoggingMiddleware>();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.MapControllers();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        Log.Information("Application started");
        app.Run();
    }
}