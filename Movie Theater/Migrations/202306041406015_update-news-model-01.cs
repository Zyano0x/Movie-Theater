namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatenewsmodel01 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "IsHome", c => c.Boolean(nullable: false));
            AddColumn("dbo.News", "IsHot", c => c.Boolean(nullable: false));
            AddColumn("dbo.News", "IsFeature", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "IsFeature");
            DropColumn("dbo.News", "IsHot");
            DropColumn("dbo.News", "IsHome");
        }
    }
}
