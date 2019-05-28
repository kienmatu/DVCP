namespace DVCP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.info",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        web_name = c.String(nullable: false, maxLength: 50),
                        web_des = c.String(maxLength: 200),
                        web_about = c.String(storeType: "ntext"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Tbl_HotPost",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        priority = c.Int(nullable: false),
                        post_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.tbl_POST", t => t.post_id)
                .Index(t => t.post_id);
            
            CreateTable(
                "dbo.tbl_POST",
                c => new
                    {
                        post_id = c.Int(nullable: false, identity: true),
                        userid = c.Int(),
                        post_title = c.String(nullable: false, maxLength: 200),
                        post_teaser = c.String(nullable: false, maxLength: 500),
                        post_review = c.String(maxLength: 500),
                        post_content = c.String(storeType: "ntext"),
                        post_type = c.Int(nullable: false),
                        post_tag = c.String(maxLength: 200),
                        create_date = c.DateTime(),
                        edit_date = c.DateTime(),
                        dynasty = c.String(maxLength: 50),
                        ViewCount = c.Int(nullable: false),
                        Rated = c.Int(nullable: false),
                        AvatarImage = c.String(maxLength: 200),
                        status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.post_id)
                .ForeignKey("dbo.tbl_User", t => t.userid)
                .Index(t => t.userid);
            
            CreateTable(
                "dbo.Tbl_Series",
                c => new
                    {
                        seriesID = c.Int(nullable: false, identity: true),
                        seriesName = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.seriesID);
            
            CreateTable(
                "dbo.tbl_User",
                c => new
                    {
                        userid = c.Int(nullable: false, identity: true),
                        username = c.String(nullable: false, maxLength: 20, unicode: false),
                        password = c.String(nullable: false, maxLength: 100, unicode: false),
                        fullname = c.String(nullable: false, maxLength: 30),
                        userrole = c.String(maxLength: 20, unicode: false),
                        status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.userid);
            
            CreateTable(
                "dbo.Tbl_SeriesPost",
                c => new
                    {
                        PostID = c.Int(nullable: false),
                        seriesID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PostID, t.seriesID })
                .ForeignKey("dbo.tbl_POST", t => t.PostID, cascadeDelete: true)
                .ForeignKey("dbo.Tbl_Series", t => t.seriesID, cascadeDelete: true)
                .Index(t => t.PostID)
                .Index(t => t.seriesID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbl_POST", "userid", "dbo.tbl_User");
            DropForeignKey("dbo.Tbl_SeriesPost", "seriesID", "dbo.Tbl_Series");
            DropForeignKey("dbo.Tbl_SeriesPost", "PostID", "dbo.tbl_POST");
            DropForeignKey("dbo.Tbl_HotPost", "post_id", "dbo.tbl_POST");
            DropIndex("dbo.Tbl_SeriesPost", new[] { "seriesID" });
            DropIndex("dbo.Tbl_SeriesPost", new[] { "PostID" });
            DropIndex("dbo.tbl_POST", new[] { "userid" });
            DropIndex("dbo.Tbl_HotPost", new[] { "post_id" });
            DropTable("dbo.Tbl_SeriesPost");
            DropTable("dbo.tbl_User");
            DropTable("dbo.Tbl_Series");
            DropTable("dbo.tbl_POST");
            DropTable("dbo.Tbl_HotPost");
            DropTable("dbo.info");
        }
    }
}
