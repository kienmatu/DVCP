namespace DVCP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addslig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_POST", "post_slug", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_POST", "post_slug");
        }
    }
}
