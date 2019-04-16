namespace DVCP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addstatus1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_User", "status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_User", "status");
        }
    }
}
