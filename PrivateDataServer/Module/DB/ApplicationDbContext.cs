using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PrivateDataServer.Module.DB.Entities;

namespace PrivateDataServer.Module.DB;

    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        /// <summary>
        /// 利用モジュール: FileModule
        /// ファイルマスタ
        /// </summary>
        public DbSet<FileMasterEntity> FileMasters { get; set; }
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
            builder.Entity<FileMasterEntity>(entity => {
                entity.ToTable("file_master");
                entity.Property(e => e.FileId)
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.CreateDate)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                 entity.Property(e => e.FileType)
                    .HasConversion<int>(); 
            });
        }
    }