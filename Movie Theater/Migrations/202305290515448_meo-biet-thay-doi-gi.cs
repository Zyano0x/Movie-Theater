namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class meobietthaydoigi : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Seats", "Room_Id", "dbo.Rooms");
            DropIndex("dbo.Seats", new[] { "Room_Id" });
            DropColumn("dbo.Seats", "Room_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Seats", "Room_Id", c => c.Int());
            CreateIndex("dbo.Seats", "Room_Id");
            AddForeignKey("dbo.Seats", "Room_Id", "dbo.Rooms", "Id");
        }
    }
}
