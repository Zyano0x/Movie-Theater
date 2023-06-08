namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabase1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MovieSchedules", "MovieId", "dbo.Movies");
            DropIndex("dbo.MovieSchedules", new[] { "MovieId" });
            CreateTable(
                "dbo.MoviePrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MatineePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TuesdayPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WeekendPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WeekPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConfirmationCode = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Seat = c.String(),
                        EarlyDiscount = c.String(),
                        Order_Id = c.Int(),
                        Showing_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .ForeignKey("dbo.Showings", t => t.Showing_Id)
                .Index(t => t.Order_Id)
                .Index(t => t.Showing_Id);
            
            CreateTable(
                "dbo.Showings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MovieId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        TheatreId = c.Int(nullable: false),
                        SpecialEventStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Movies", t => t.MovieId, cascadeDelete: true)
                .ForeignKey("dbo.Theatres", t => t.TheatreId, cascadeDelete: true)
                .Index(t => t.MovieId)
                .Index(t => t.TheatreId);
            
            CreateTable(
                "dbo.Theatres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Seats", "Name", c => c.String());
            DropColumn("dbo.Seats", "State");
            DropColumn("dbo.Seats", "Cost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Seats", "Cost", c => c.Long(nullable: false));
            AddColumn("dbo.Seats", "State", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Orders", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tickets", "Showing_Id", "dbo.Showings");
            DropForeignKey("dbo.Showings", "TheatreId", "dbo.Theatres");
            DropForeignKey("dbo.Showings", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.Tickets", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Showings", new[] { "TheatreId" });
            DropIndex("dbo.Showings", new[] { "MovieId" });
            DropIndex("dbo.Tickets", new[] { "Showing_Id" });
            DropIndex("dbo.Tickets", new[] { "Order_Id" });
            DropIndex("dbo.Orders", new[] { "User_Id" });
            DropColumn("dbo.Seats", "Name");
            DropTable("dbo.Theatres");
            DropTable("dbo.Showings");
            DropTable("dbo.Tickets");
            DropTable("dbo.Orders");
            DropTable("dbo.MoviePrices");
            CreateIndex("dbo.MovieSchedules", "MovieId");
            AddForeignKey("dbo.MovieSchedules", "MovieId", "dbo.Movies", "Id", cascadeDelete: true);
        }
    }
}
