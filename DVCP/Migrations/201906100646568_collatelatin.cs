namespace DVCP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class collatelatin : DbMigration
    {
        string dbname = new DVCP.Models.DVCPContext().Database.Connection.Database;
        public override void Up()
        {
            Sql("ALTER TABLE dbo.Tbl_POST ALTER column [post_title] nvarchar(200) COLLATE  SQL_Latin1_General_CP1_CI_AI", true);
        }
        
        public override void Down()
        {
        }
    }
}
