namespace DVCP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addratedtoPost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_POST", "Rated", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_POST", "Rated");
        }
    }
}
