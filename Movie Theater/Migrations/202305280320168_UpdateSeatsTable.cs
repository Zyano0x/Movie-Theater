namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Seats", "MovieId", "dbo.Movies");
            DropIndex("dbo.Seats", new[] { "MovieId" });
            AlterColumn("dbo.Seats", "Cost", c => c.Long(nullable: false));
            DropColumn("dbo.Seats", "MovieId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Seats", "MovieId", c => c.Int());
            AlterColumn("dbo.Seats", "Cost", c => c.Int(nullable: false));
            CreateIndex("dbo.Seats", "MovieId");
            AddForeignKey("dbo.Seats", "MovieId", "dbo.Movies", "Id");
        }
    }
}
