namespace DVCP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialdata : DbMigration
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
                    })
                .PrimaryKey(t => t.post_id)
                .ForeignKey("dbo.tbl_User", t => t.userid)
                .Index(t => t.userid);
            
            CreateTable(
                "dbo.tbl_User",
                c => new
                    {
                        userid = c.Int(nullable: false, identity: true),
                        username = c.String(nullable: false, maxLength: 20, unicode: false),
                        password = c.String(nullable: false, maxLength: 100, unicode: false),
                        fullname = c.String(nullable: false, maxLength: 30),
                        userrole = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.userid)
                .Index(t => t.username, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbl_POST", "userid", "dbo.tbl_User");
            DropIndex("dbo.tbl_User", new[] { "username" });
            DropIndex("dbo.tbl_POST", new[] { "userid" });
            DropTable("dbo.tbl_User");
            DropTable("dbo.tbl_POST");
            DropTable("dbo.info");
        }
    }
}
