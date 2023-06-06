namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatenewsmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "ModifireDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "ModifireDate");
        }
    }
}
