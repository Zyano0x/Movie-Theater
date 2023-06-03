namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RoomSeats", "RoomId", "dbo.Rooms");
            DropForeignKey("dbo.RoomSeats", "SeatId", "dbo.Seats");
            DropIndex("dbo.RoomSeats", new[] { "RoomId" });
            DropIndex("dbo.RoomSeats", new[] { "SeatId" });
            DropTable("dbo.RoomSeats");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RoomSeats",
                c => new
                    {
                        RoomId = c.Int(nullable: false),
                        SeatId = c.Int(nullable: false),
                        Seat_State = c.Boolean(nullable: false),
                        Seat_State_Book = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoomId, t.SeatId });
            
            CreateIndex("dbo.RoomSeats", "SeatId");
            CreateIndex("dbo.RoomSeats", "RoomId");
            AddForeignKey("dbo.RoomSeats", "SeatId", "dbo.Seats", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RoomSeats", "RoomId", "dbo.Rooms", "Id", cascadeDelete: true);
        }
    }
}
