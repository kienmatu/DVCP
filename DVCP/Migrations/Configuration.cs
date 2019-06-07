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

        protected override void Seed(DVCPContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.info.AddOrUpdate( x =>x.id,
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
                    password = "0192023A7BBD73250516F069DF18B500", // = admin123
                    fullname = "ADMIN ĐVCP",
                    userrole = "admin",
                    status = true,
                }
                );
            context.Tbl_Tags.AddOrUpdate(x => x.TagID,
                new Tbl_Tags { TagID = 1, TagName = "Kiến trúc" },
                new Tbl_Tags { TagID = 2, TagName = "Chất liệu" },
                new Tbl_Tags { TagID = 3, TagName = "Binh bị" },
                new Tbl_Tags { TagID = 4, TagName = "Quân sự" },
                new Tbl_Tags { TagID = 5, TagName = "Thần thoại" },
                new Tbl_Tags { TagID = 6, TagName = "Văn hóa" },
                new Tbl_Tags { TagID = 7, TagName = "Phong tục" },
                new Tbl_Tags { TagID = 8, TagName = "Tôn giáo" },
                new Tbl_Tags { TagID = 9, TagName = "Trang phục" }
                );
            context.SaveChanges();
        }
    }
}
