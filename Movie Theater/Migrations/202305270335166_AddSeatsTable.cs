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
            
            AddColumn("dbo.MovieTickets", "SeatId", c => c.Int());
            CreateIndex("dbo.MovieTickets", "SeatId");
            AddForeignKey("dbo.MovieTickets", "SeatId", "dbo.Seats", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MovieTickets", "SeatId", "dbo.Seats");
            DropForeignKey("dbo.Seats", "MovieId", "dbo.Movies");
            DropIndex("dbo.Seats", new[] { "MovieId" });
            DropIndex("dbo.MovieTickets", new[] { "SeatId" });
            DropColumn("dbo.MovieTickets", "SeatId");
            DropTable("dbo.Seats");
        }
    }
}
