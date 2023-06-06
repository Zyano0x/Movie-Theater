namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateMoviePrice : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO MoviePrices (MatineePrice, TuesdayPrice, WeekendPrice, WeekPrice) VALUES (140000, 150000, 165000, 165000)");
        }
        
        public override void Down()
        {
        }
    }
}
