using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie_Theater.Models.Utilities
{
    public class GenerateConfirmationCode
    {
        public static int GetNextConfirmationCode()
        {
            //we need a db context to connect to the database
            ApplicationDbContext _dbContext = new ApplicationDbContext();

            int intMaxConfirmationCode; //the current maximum Order number
            int intNextConfirmationCode; //the Order number for the next Product

            if (_dbContext.Orders.Count() == 0) //there are no registrations in the database yet
            { 
                intMaxConfirmationCode = 10000; //registration numbers start at 101
            }
            else
            {
                intMaxConfirmationCode = _dbContext.Orders.Max(c => c.ConfirmationCode); //this is the highest number in the database right now
            }

            //add one to the current max to find the next one
            intNextConfirmationCode = intMaxConfirmationCode + 1;

            //return the value
            return intNextConfirmationCode;
        }
    }
}