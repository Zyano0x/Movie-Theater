namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bigupdatedatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Slider",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Img = c.String(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.MovieSchedules", "MovieTitle", c => c.String(nullable: false));
            AlterColumn("dbo.Crews", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.Movies", "Title", c => c.String(maxLength: 255));
            AlterColumn("dbo.Movies", "Rating", c => c.Int(nullable: false));
            AlterColumn("dbo.Movies", "TrailerUrl", c => c.String());
            DropColumn("dbo.Movies", "OriginalLanguage");
            DropColumn("dbo.Movies", "BoxOffice");
            DropColumn("dbo.Movies", "Distributor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "Distributor", c => c.String());
            AddColumn("dbo.Movies", "BoxOffice", c => c.Single(nullable: false));
            AddColumn("dbo.Movies", "OriginalLanguage", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "TrailerUrl", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Rating", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Title", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Crews", "Name", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.MovieSchedules", "MovieTitle");
            DropTable("dbo.Slider");
        }
    }
}
