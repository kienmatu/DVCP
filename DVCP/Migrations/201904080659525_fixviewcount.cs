namespace DVCP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixviewcount : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_POST", "ViewCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_POST", "ViewCount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
