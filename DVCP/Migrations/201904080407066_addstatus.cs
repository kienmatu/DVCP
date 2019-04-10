namespace DVCP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addstatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_POST", "status", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_POST", "status");
        }
    }
}
