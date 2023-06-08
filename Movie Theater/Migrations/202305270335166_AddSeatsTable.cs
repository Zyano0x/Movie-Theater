namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSeatsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Seats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.Boolean(nullable: false),
                        Cost = c.Int(nullable: false),
                        MovieId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Movies", t => t.MovieId)
                .Index(t => t.MovieId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Seats", "MovieId", "dbo.Movies");
            DropIndex("dbo.Seats", new[] { "MovieId" });
            DropTable("dbo.Seats");
        }
    }
}
