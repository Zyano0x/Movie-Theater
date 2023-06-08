namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReview : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Reviews");
            AddColumn("dbo.Reviews", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Reviews", "UserId", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.Reviews", "Id");
            CreateIndex("dbo.Reviews", "UserId");
            AddForeignKey("dbo.Reviews", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropPrimaryKey("dbo.Reviews");
            AlterColumn("dbo.Reviews", "UserId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Reviews", "Id");
            AddPrimaryKey("dbo.Reviews", new[] { "MovieId", "UserId" });
        }
    }
}
