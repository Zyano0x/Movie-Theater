namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDatabase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tickets", "SeatId", "dbo.Seats");
            DropForeignKey("dbo.Tickets", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Tickets", new[] { "UserId" });
            DropIndex("dbo.Tickets", new[] { "SeatId" });
            DropTable("dbo.Tickets");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.String(),
                        UserId = c.String(maxLength: 128),
                        MovieTitle = c.String(),
                        TimeBooking = c.DateTime(nullable: false),
                        SeatId = c.Int(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Tickets", "SeatId");
            CreateIndex("dbo.Tickets", "UserId");
            AddForeignKey("dbo.Tickets", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Tickets", "SeatId", "dbo.Seats", "Id");
        }
    }
}
