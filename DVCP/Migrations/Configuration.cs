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
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DVCPContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.WebInfo.AddOrUpdate( x =>x.id,
                new WebInfo
                {
                    id = 1,
                    web_name = "Đại Việt Cổ Phong",
                    web_des = "Trang web chính thức của Đại Việt Cổ Phong",
                    web_about = "Về Đại Việt Cổ Phong",
                }
                );
            context.Users.AddOrUpdate(x => x.username,
                new User
                {
                    username = "admin",
                    password = "0192023A7BBD73250516F069DF18B500", // = admin123
                    fullname = "ADMIN ĐVCP",
                    userrole = "admin",
                    status = true,
                }
                );
            context.Tags.AddOrUpdate(x => x.TagID,
                new Tag { TagID = 1, TagName = "Kiến trúc" },
                new Tag { TagID = 2, TagName = "Chất liệu" },
                new Tag { TagID = 3, TagName = "Binh bị" },
                new Tag { TagID = 4, TagName = "Quân sự" },
                new Tag { TagID = 5, TagName = "Thần thoại" },
                new Tag { TagID = 6, TagName = "Văn hóa" },
                new Tag { TagID = 7, TagName = "Phong tục" },
                new Tag { TagID = 8, TagName = "Tôn giáo" },
                new Tag { TagID = 9, TagName = "Trang phục" }
                );
            context.SaveChanges();
        }
    }
}
