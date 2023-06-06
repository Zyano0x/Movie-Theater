namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changenewmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "Alias", c => c.String());
            AddColumn("dbo.News", "Detail", c => c.String());
            AddColumn("dbo.News", "SeoTitle", c => c.String());
            AddColumn("dbo.News", "SeoDescription", c => c.String());
            AddColumn("dbo.News", "SeoKeywords", c => c.String());
            AlterColumn("dbo.News", "Img", c => c.String());
            AlterColumn("dbo.News", "Title", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.News", "Description", c => c.String());
            DropColumn("dbo.News", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.News", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.News", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.News", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.News", "Img", c => c.String(nullable: false));
            DropColumn("dbo.News", "SeoKeywords");
            DropColumn("dbo.News", "SeoDescription");
            DropColumn("dbo.News", "SeoTitle");
            DropColumn("dbo.News", "Detail");
            DropColumn("dbo.News", "Alias");
        }
    }
}
