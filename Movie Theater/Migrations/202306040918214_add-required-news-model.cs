namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrequirednewsmodel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "Img", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.News", "Img", c => c.String());
        }
    }
}
