namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateNewsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "Url");
        }
    }
}
