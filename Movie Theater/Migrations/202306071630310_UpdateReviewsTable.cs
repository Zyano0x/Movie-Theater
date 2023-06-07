namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReviewsTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reviews", "MovieId", "dbo.Movies");
            DropIndex("dbo.Reviews", new[] { "MovieId" });
            DropPrimaryKey("dbo.Reviews");
            AddColumn("dbo.Reviews", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Reviews", new[] { "Id", "MovieId", "UserId" });
            CreateIndex("dbo.Reviews", "Id");
            AddForeignKey("dbo.Reviews", "Id", "dbo.Movies", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "Id", "dbo.Movies");
            DropIndex("dbo.Reviews", new[] { "Id" });
            DropPrimaryKey("dbo.Reviews");
            DropColumn("dbo.Reviews", "Id");
            AddPrimaryKey("dbo.Reviews", new[] { "MovieId", "UserId" });
            CreateIndex("dbo.Reviews", "MovieId");
            AddForeignKey("dbo.Reviews", "MovieId", "dbo.Movies", "Id", cascadeDelete: false);
        }
    }
}
