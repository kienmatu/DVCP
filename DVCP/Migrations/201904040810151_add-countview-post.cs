namespace DVCP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcountviewpost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_POST", "ViewCount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_POST", "ViewCount");
        }
    }
}
