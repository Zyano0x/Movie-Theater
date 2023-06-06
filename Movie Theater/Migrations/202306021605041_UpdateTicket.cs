namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTicket : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MovieTickets", "SeatId", "dbo.Seats");
            DropIndex("dbo.MovieTickets", new[] { "SeatId" });
            DropColumn("dbo.MovieTickets", "Quantity");
            DropColumn("dbo.MovieTickets", "SeatId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MovieTickets", "SeatId", c => c.Int());
            AddColumn("dbo.MovieTickets", "Quantity", c => c.Int(nullable: false));
            CreateIndex("dbo.MovieTickets", "SeatId");
            AddForeignKey("dbo.MovieTickets", "SeatId", "dbo.Seats", "Id");
        }
    }
}
