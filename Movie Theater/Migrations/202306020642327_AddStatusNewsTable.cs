namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusNewsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "IsActive");
        }
    }
}
