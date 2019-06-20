namespace DVCP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addslUg : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Tbl_POST", "post_slug", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tbl_POST", new[] { "post_slug" });
        }
    }
}
