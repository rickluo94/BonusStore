using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BonusStore.Model;

namespace BonusStore.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        #region 專案產生實體資料庫的語法
        //public string DbPath { get; }
        //public ApplicationDbContext()
        //{
        //    //使用dotnet ef參考一下路徑建立測試資料庫
        //    // 產生創建資料庫所需的描述檔 add 後面不要用中文會當機
        //    // dotnet ef migrations add InitialCreate
        //    // 創建資料庫
        //    // dotnet ef database update
        //    var folder = Environment.SpecialFolder.LocalApplicationData;
        //    var path = Environment.GetFolderPath(folder);
        //    DbPath = System.IO.Path.Join(path, "app.db");
        //}

        ////The following configures EF to create a Sqlite database file in the
        ////special "local" folder for your platform.

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlite($"Data Source={DbPath}");

        #endregion

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Loyalty> Loyalties { get; set; }
    }
}

