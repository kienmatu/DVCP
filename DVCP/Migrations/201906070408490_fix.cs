namespace DVCP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tbl_POST", "post_tag");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tbl_POST", "post_tag", c => c.String(maxLength: 200));
        }
    }
}
