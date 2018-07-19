using System.Diagnostics;
using NewNodeChecker.Models;

namespace NewNodeChecker.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NewNodeChecker.Database.LogDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(NewNodeChecker.Database.LogDbContext context)
        {

            string defaultDefinationSettingName = "Default";
            DefinationSetting oDefinationSetting = null;
            try
            {
                oDefinationSetting = new DefinationSetting() {Name = defaultDefinationSettingName};
                context.DefinationSettings.AddOrUpdate(x => x.Name, oDefinationSetting);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            oDefinationSetting = (from d in context.DefinationSettings
                where d.Name == defaultDefinationSettingName
                select d).FirstOrDefault();

            SqlConnectionDefination oSqlConnectionDefination = new SqlConnectionDefination()
            {
                Name = "Default",
                SqlConnection =
                    @"Data Source=.\atu;Initial Catalog=NewNodeCheckerDB;Integrated Security=SSPI;MultipleActiveResultSets=true;",
                IsEnabled = true,
                SqlStatment = "select getdate();",
                DefinationSettingId = oDefinationSetting.Id
            };
            context.SqlConnectionDefinations.AddOrUpdate(x=>x.Name,oSqlConnectionDefination);


            //PortInfoDefination oPortInfoDefination = new PortInfoDefination()
            //{
            //    Name = "Taha PC",
            //    Ip4Address = "10.2.80.236",
            //    PortNo = 65000,
            //    IsEnabled = true,
            //    DefinationSetting = oDefinationSetting
            //};
            //context.PortInfoDefinations.AddOrUpdate(x =>new {x.Name,x.Ip4Address,x.PortNo}, oPortInfoDefination);
            //oPortInfoDefination = new PortInfoDefination()
            //{
            //    Name = "Taha PC",
            //    Ip4Address = "10.2.80.236",
            //    PortNo = 80,
            //    IsEnabled = true,
            //    DefinationSetting = oDefinationSetting
            //};
            //context.PortInfoDefinations.AddOrUpdate(x => new { x.Name, x.Ip4Address, x.PortNo }, oPortInfoDefination);
            context.SaveChanges();
        }
    }
}
