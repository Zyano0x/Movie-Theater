namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _02 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        State = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoomSeats",
                c => new
                    {
                        RoomId = c.Int(nullable: false),
                        SeatId = c.Int(nullable: false),
                        Seat_State = c.Boolean(nullable: false),
                        Seat_State_Book = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoomId, t.SeatId })
                .ForeignKey("dbo.Rooms", t => t.RoomId, cascadeDelete: true)
                .ForeignKey("dbo.Seats", t => t.SeatId, cascadeDelete: true)
                .Index(t => t.RoomId)
                .Index(t => t.SeatId);
            
            AddColumn("dbo.Seats", "Room_Id", c => c.Int());
            CreateIndex("dbo.Seats", "Room_Id");
            AddForeignKey("dbo.Seats", "Room_Id", "dbo.Rooms", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoomSeats", "SeatId", "dbo.Seats");
            DropForeignKey("dbo.RoomSeats", "RoomId", "dbo.Rooms");
            DropForeignKey("dbo.Seats", "Room_Id", "dbo.Rooms");
            DropIndex("dbo.RoomSeats", new[] { "SeatId" });
            DropIndex("dbo.RoomSeats", new[] { "RoomId" });
            DropIndex("dbo.Seats", new[] { "Room_Id" });
            DropColumn("dbo.Seats", "Room_Id");
            DropTable("dbo.RoomSeats");
            DropTable("dbo.Rooms");
        }
    }
}
