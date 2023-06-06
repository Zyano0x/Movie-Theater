namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangessDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.News",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Img = c.String(nullable: false),
                    Title = c.String(nullable: false),
                    Detail = c.String(nullable: false),
                    Author = c.String(nullable: true),
                    Description = c.String(nullable: true),
                    PublicationDate = c.DateTime(nullable: false),
                    SeoTitle = c.String(),
                    SeoDescription = c.String(),
                    SeoKeywords = c.String(),
                    ModificationDate = c.DateTime(nullable: false),
                    IsActive = c.Boolean(nullable: false),
                    IsHome = c.Boolean(nullable: false),
                    IsHot = c.Boolean(nullable: false),
                    IsFeature = c.Boolean(nullable: false),
                    Url = c.String(),
                })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.News");
        }
    }
}
