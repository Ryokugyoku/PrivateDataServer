using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace PrivateDataServer.Module.DB;

    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // データベースが存在しない場合にのみマイグレーションを実行
            if (Database.GetPendingMigrations().Any())
            {
                Database.Migrate();
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Identity のテーブルを正しく作成するための設定を追加
        }
    }