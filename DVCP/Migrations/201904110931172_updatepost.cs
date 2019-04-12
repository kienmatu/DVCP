namespace DVCP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatepost : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_POST", "status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_POST", "status", c => c.Boolean());
        }
    }
}
