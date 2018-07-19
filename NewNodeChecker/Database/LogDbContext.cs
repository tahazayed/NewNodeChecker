using System.Data.Entity;
using NewNodeChecker.Models;

namespace NewNodeChecker.Database
{
    public class LogDbContext : DbContext
    {
        public LogDbContext()
            : base("NewNodeCheckerDB")
        {
                System.Data.Entity.Database.SetInitializer<LogDbContext>(null);
        }
        public DbSet<WebSiteFileLog> WebSiteFileLogs { get; set; }
        public DbSet<ServerLog> ServerLogs { get; set; }
        public DbSet<InstalledAppLog> InstalledAppsLogs { get; set; }
        public DbSet<WebSiteLog> WebSiteLogs { get; set; }
        public DbSet<HostsFileLog> HostsFileLogs { get; set; }
        public DbSet<SqlConnectionDefination> SqlConnectionDefinations { get; set; }
        public DbSet<SqlTransResultLog> SqlTransResultLogs { get; set; }
        public DbSet<PortInfoDefination> PortInfoDefinations { get; set; }
        public DbSet<PortResultLog> PortResultLogs { get; set; }
        public DbSet<DefinationSetting> DefinationSettings { get; set; }
        public DbSet<ConfigFileLog> ConfigFileLogs { get; set; }
        public DbSet<ConfigConnectionStringLog> ConfigConnectionStringLog { get; set; }
        public DbSet<ConfigLinksDefinition> ConfigLinksDefinition { get; set; }
        public DbSet<ConfigIPLog> ConfigIPLog { get; set; }
        public DbSet<ConfigLinksLog> ConfigLinksLog { get; set; }
        public DbSet<ConfigURLBridge> ConfigURLBridge { get; set; }
        public DbSet<RunStepLog> RunStepLog { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
