namespace NewNodeChecker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConfigConnectionStringLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConfigFileLogId = c.Int(nullable: false),
                        SqlConnectionDefinationId = c.Int(nullable: false),
                        ConnectionSting = c.String(nullable: false),
                        RowVesion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        EventDateTime = c.DateTime(nullable: false),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ConfigFileLogs", t => t.ConfigFileLogId, cascadeDelete: true)
                .ForeignKey("dbo.SqlConnectionDefinations", t => t.SqlConnectionDefinationId, cascadeDelete: true)
                .Index(t => t.ConfigFileLogId)
                .Index(t => t.SqlConnectionDefinationId);
            
            CreateTable(
                "dbo.ConfigFileLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WebSiteLogId = c.Int(nullable: false),
                        Type = c.String(),
                        ConfigFileName = c.String(nullable: false),
                        ConfigFileContent = c.String(storeType: "xml"),
                        LastModificationDate = c.DateTime(),
                        RowVesion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        EventDateTime = c.DateTime(nullable: false),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WebSiteLogs", t => t.WebSiteLogId, cascadeDelete: true)
                .Index(t => t.WebSiteLogId);
            
            CreateTable(
                "dbo.WebSiteLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppPhysicalPath = c.String(nullable: false),
                        SiteName = c.String(),
                        VirtualDirectoryName = c.String(),
                        ServerLogId = c.Int(nullable: false),
                        RowVesion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        EventDateTime = c.DateTime(nullable: false),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServerLogs", t => t.ServerLogId, cascadeDelete: true)
                .Index(t => t.ServerLogId);
            
            CreateTable(
                "dbo.ServerLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MachineName = c.String(nullable: false, maxLength: 500),
                        Ip = c.String(),
                        StartDateTime = c.DateTime(),
                        EndDateTime = c.DateTime(),
                        RowVesion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        EventDateTime = c.DateTime(nullable: false),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HostsFileLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileContent = c.String(nullable: false),
                        ServerLogId = c.Int(nullable: false),
                        RowVesion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        EventDateTime = c.DateTime(nullable: false),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServerLogs", t => t.ServerLogId, cascadeDelete: true)
                .Index(t => t.ServerLogId);
            
            CreateTable(
                "dbo.InstalledAppLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplayName = c.String(maxLength: 500),
                        DisplayVersion = c.String(maxLength: 100),
                        InstallDate = c.DateTime(),
                        InstallSource = c.String(),
                        ServerLogId = c.Int(nullable: false),
                        RowVesion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        EventDateTime = c.DateTime(nullable: false),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServerLogs", t => t.ServerLogId, cascadeDelete: true)
                .Index(t => t.DisplayName)
                .Index(t => t.DisplayVersion)
                .Index(t => t.ServerLogId);
            
            CreateTable(
                "dbo.SqlTransResultLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SqlConnectionDefinationId = c.Int(nullable: false),
                        RowsCount = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        ServerLogId = c.Int(nullable: false),
                        RowVesion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        EventDateTime = c.DateTime(nullable: false),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServerLogs", t => t.ServerLogId, cascadeDelete: true)
                .ForeignKey("dbo.SqlConnectionDefinations", t => t.SqlConnectionDefinationId, cascadeDelete: true)
                .Index(t => t.SqlConnectionDefinationId)
                .Index(t => t.ServerLogId);
            
            CreateTable(
                "dbo.SqlConnectionDefinations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SqlConnection = c.String(nullable: false),
                        SqlStatment = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200),
                        IsEnabled = c.Boolean(nullable: false),
                        DefinationSettingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DefinationSettings", t => t.DefinationSettingId, cascadeDelete: true)
                .Index(t => t.DefinationSettingId);
            
            CreateTable(
                "dbo.DefinationSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        RowVesion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        EventDateTime = c.DateTime(nullable: false),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WebSiteFileLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WebSiteLogId = c.Int(nullable: false),
                        PhysicalPath = c.String(),
                        FileName = c.String(nullable: false),
                        Extension = c.String(),
                        Size = c.Long(nullable: false),
                        BuildNo = c.String(),
                        LastModificationDate = c.DateTime(),
                        RowVesion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        EventDateTime = c.DateTime(nullable: false),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WebSiteLogs", t => t.WebSiteLogId, cascadeDelete: true)
                .Index(t => t.WebSiteLogId);
            
            CreateTable(
                "dbo.ConfigIPLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConfigFileLogId = c.Int(nullable: false),
                        IP = c.String(nullable: false),
                        Key = c.String(),
                        RowVesion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        EventDateTime = c.DateTime(nullable: false),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ConfigFileLogs", t => t.ConfigFileLogId, cascadeDelete: true)
                .Index(t => t.ConfigFileLogId);
            
            CreateTable(
                "dbo.ConfigLinksDefinitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Links = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200),
                        IsEnabled = c.Boolean(nullable: false),
                        DefinationSettingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DefinationSettings", t => t.DefinationSettingId, cascadeDelete: true)
                .Index(t => t.DefinationSettingId);
            
            CreateTable(
                "dbo.ConfigLinksLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConfigLinksDefinitionId = c.Int(nullable: false),
                        Status = c.String(nullable: false),
                        ServerLogId = c.Int(nullable: false),
                        RowVesion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        EventDateTime = c.DateTime(nullable: false),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ConfigLinksDefinitions", t => t.ConfigLinksDefinitionId, cascadeDelete: true)
                .ForeignKey("dbo.ServerLogs", t => t.ServerLogId, cascadeDelete: true)
                .Index(t => t.ConfigLinksDefinitionId)
                .Index(t => t.ServerLogId);
            
            CreateTable(
                "dbo.ConfigURLBridges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConfigId = c.Int(nullable: false),
                        URLId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PortInfoDefinations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ip4Address = c.String(nullable: false),
                        PortNo = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200),
                        IsEnabled = c.Boolean(nullable: false),
                        DefinationSettingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DefinationSettings", t => t.DefinationSettingId, cascadeDelete: true)
                .Index(t => t.DefinationSettingId);
            
            CreateTable(
                "dbo.PortResultLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PortId = c.Int(nullable: false),
                        IsOpened = c.Boolean(nullable: false),
                        ServerLogId = c.Int(nullable: false),
                        RowVesion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        EventDateTime = c.DateTime(nullable: false),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PortInfoDefinations", t => t.PortId, cascadeDelete: true)
                .ForeignKey("dbo.ServerLogs", t => t.ServerLogId, cascadeDelete: true)
                .Index(t => t.PortId)
                .Index(t => t.ServerLogId);
            
            CreateTable(
                "dbo.RunStepLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StepName = c.String(),
                        StartDateTime = c.DateTime(),
                        EndDateTime = c.DateTime(),
                        ServerLogId = c.Int(nullable: false),
                        RowVesion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        EventDateTime = c.DateTime(nullable: false),
                        Exception = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServerLogs", t => t.ServerLogId, cascadeDelete: true)
                .Index(t => t.ServerLogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RunStepLogs", "ServerLogId", "dbo.ServerLogs");
            DropForeignKey("dbo.PortResultLogs", "ServerLogId", "dbo.ServerLogs");
            DropForeignKey("dbo.PortResultLogs", "PortId", "dbo.PortInfoDefinations");
            DropForeignKey("dbo.PortInfoDefinations", "DefinationSettingId", "dbo.DefinationSettings");
            DropForeignKey("dbo.ConfigLinksLogs", "ServerLogId", "dbo.ServerLogs");
            DropForeignKey("dbo.ConfigLinksLogs", "ConfigLinksDefinitionId", "dbo.ConfigLinksDefinitions");
            DropForeignKey("dbo.ConfigLinksDefinitions", "DefinationSettingId", "dbo.DefinationSettings");
            DropForeignKey("dbo.ConfigIPLogs", "ConfigFileLogId", "dbo.ConfigFileLogs");
            DropForeignKey("dbo.ConfigConnectionStringLogs", "SqlConnectionDefinationId", "dbo.SqlConnectionDefinations");
            DropForeignKey("dbo.ConfigConnectionStringLogs", "ConfigFileLogId", "dbo.ConfigFileLogs");
            DropForeignKey("dbo.ConfigFileLogs", "WebSiteLogId", "dbo.WebSiteLogs");
            DropForeignKey("dbo.WebSiteFileLogs", "WebSiteLogId", "dbo.WebSiteLogs");
            DropForeignKey("dbo.WebSiteLogs", "ServerLogId", "dbo.ServerLogs");
            DropForeignKey("dbo.SqlTransResultLogs", "SqlConnectionDefinationId", "dbo.SqlConnectionDefinations");
            DropForeignKey("dbo.SqlConnectionDefinations", "DefinationSettingId", "dbo.DefinationSettings");
            DropForeignKey("dbo.SqlTransResultLogs", "ServerLogId", "dbo.ServerLogs");
            DropForeignKey("dbo.InstalledAppLogs", "ServerLogId", "dbo.ServerLogs");
            DropForeignKey("dbo.HostsFileLogs", "ServerLogId", "dbo.ServerLogs");
            DropIndex("dbo.RunStepLogs", new[] { "ServerLogId" });
            DropIndex("dbo.PortResultLogs", new[] { "ServerLogId" });
            DropIndex("dbo.PortResultLogs", new[] { "PortId" });
            DropIndex("dbo.PortInfoDefinations", new[] { "DefinationSettingId" });
            DropIndex("dbo.ConfigLinksLogs", new[] { "ServerLogId" });
            DropIndex("dbo.ConfigLinksLogs", new[] { "ConfigLinksDefinitionId" });
            DropIndex("dbo.ConfigLinksDefinitions", new[] { "DefinationSettingId" });
            DropIndex("dbo.ConfigIPLogs", new[] { "ConfigFileLogId" });
            DropIndex("dbo.WebSiteFileLogs", new[] { "WebSiteLogId" });
            DropIndex("dbo.SqlConnectionDefinations", new[] { "DefinationSettingId" });
            DropIndex("dbo.SqlTransResultLogs", new[] { "ServerLogId" });
            DropIndex("dbo.SqlTransResultLogs", new[] { "SqlConnectionDefinationId" });
            DropIndex("dbo.InstalledAppLogs", new[] { "ServerLogId" });
            DropIndex("dbo.InstalledAppLogs", new[] { "DisplayVersion" });
            DropIndex("dbo.InstalledAppLogs", new[] { "DisplayName" });
            DropIndex("dbo.HostsFileLogs", new[] { "ServerLogId" });
            DropIndex("dbo.WebSiteLogs", new[] { "ServerLogId" });
            DropIndex("dbo.ConfigFileLogs", new[] { "WebSiteLogId" });
            DropIndex("dbo.ConfigConnectionStringLogs", new[] { "SqlConnectionDefinationId" });
            DropIndex("dbo.ConfigConnectionStringLogs", new[] { "ConfigFileLogId" });
            DropTable("dbo.RunStepLogs");
            DropTable("dbo.PortResultLogs");
            DropTable("dbo.PortInfoDefinations");
            DropTable("dbo.ConfigURLBridges");
            DropTable("dbo.ConfigLinksLogs");
            DropTable("dbo.ConfigLinksDefinitions");
            DropTable("dbo.ConfigIPLogs");
            DropTable("dbo.WebSiteFileLogs");
            DropTable("dbo.DefinationSettings");
            DropTable("dbo.SqlConnectionDefinations");
            DropTable("dbo.SqlTransResultLogs");
            DropTable("dbo.InstalledAppLogs");
            DropTable("dbo.HostsFileLogs");
            DropTable("dbo.ServerLogs");
            DropTable("dbo.WebSiteLogs");
            DropTable("dbo.ConfigFileLogs");
            DropTable("dbo.ConfigConnectionStringLogs");
        }
    }
}
