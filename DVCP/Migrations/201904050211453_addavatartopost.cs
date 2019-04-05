namespace DVCP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addavatartopost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_POST", "AvatarImage", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_POST", "AvatarImage");
        }
    }
}
