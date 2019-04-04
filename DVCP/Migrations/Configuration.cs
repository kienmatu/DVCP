namespace DVCP.Migrations
{
    using DVCP.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DVCP.Models.DVCPContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DVCP.Models.DVCPContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.infoes.AddOrUpdate( x =>x.id,
                new info
                {
                    id = 1,
                    web_name = "Đại Việt Cổ Phong",
                    web_des = "Trang web chính thức của Đại Việt Cổ Phong",
                    web_about = "Về Đại Việt Cổ Phong",
                }
                );
            context.tbl_User.AddOrUpdate(x => x.username,
                new tbl_User
                {
                    username = "admin",
                    password = "admin",
                    fullname = "ADMIN ĐVCP",
                    userrole = "admin",

                }
                );
            context.SaveChanges();
        }
    }
}
