using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie_Theater.Models.Utilities
{
    public class GenerateTicketPrice
    {
        public static Decimal GetTicketPrice(DateTime ShowDate)

        {
            //we need a db context to connect to the database
            ApplicationDbContext _dbContext = new ApplicationDbContext();

            //Create return value 
            decimal TicketPrice;

            ////Initiate bol values to figure out what day/time it is
            Boolean bolWeekend = false; //Variable to check whether the current day is a Weekday(M-F)
            Boolean bolMatinee = false; //Variable to check whether current time < 12:00pm
            Boolean bolFriday = false; //Variable to check whether current day is Friday, because half of friday is the weekend
            Boolean bolTuesday = false; //Variable to check whether it is a discount day or not
            Boolean bolBefore5 = false; //Variable to check whether it is = or < 5pm

            //Create movieprice object that references the most recent record of the MoviePriceID
            MoviePrice movieprice = _dbContext.MoviePrices.FirstOrDefault(x => x.id == 1);

            ////Get prices of different showings to be able to compare and populate booleans
            Decimal MoviePriceMat = movieprice.MatineePrice;
            Decimal MoviePriceWeek = movieprice.WeekPrice;
            Decimal MoviePriceWeeknd = movieprice.WeekendPrice;
            Decimal MoviePriceTues = movieprice.TuesdayPrice;

            //Convert showtime date into a comparable type 

            String strday = ShowDate.DayOfWeek.ToString();
            Int32 inthour = ShowDate.Hour;

            //Use Decisions statements to accurately populate booleans
            if (strday == "Friday")
            {
                bolFriday = true;
            }

            if ((strday == "Saturday") || (strday == "Sunday"))
            {
                bolWeekend = true;
            }

            if (strday == "Tuesday")
            {
                bolTuesday = true;
            }

            if (inthour < 12)
            {
                bolMatinee = true;
            }
            else if (inthour < 17)
            {
                bolBefore5 = true;
            }

            ////Filter and process through booleans, Call data from Ticket Price, and assign appropriate values to decTicketPrice
            if ((bolTuesday) && (bolBefore5))  // Check if discount rate applies (Tuesday and before 5pm)
            {
                TicketPrice = MoviePriceTues;
            }
            else if ((bolWeekend) || (bolFriday && !(bolMatinee)))  //Check if it is a weekend (friday > 12pm through Sunday evening)
            {
                TicketPrice = MoviePriceWeeknd;
            }
            else if (bolMatinee) //Checks if time is before 12pm and through process of elimination falls on a weekday
            {
                TicketPrice = MoviePriceMat;
            }
            else //Handles all weekdays after 12pm
            {
                TicketPrice = MoviePriceWeek;
            }

            //return the value
            return TicketPrice;
        }
    }
}