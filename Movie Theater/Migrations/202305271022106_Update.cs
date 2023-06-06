namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Seats", t => t.SeatId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.SeatId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tickets", "SeatId", "dbo.Seats");
            DropIndex("dbo.Tickets", new[] { "SeatId" });
            DropIndex("dbo.Tickets", new[] { "UserId" });
            DropTable("dbo.Tickets");
        }
    }
}
