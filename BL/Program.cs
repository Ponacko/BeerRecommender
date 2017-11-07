using BeerRecommender;
using BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    class Program
    {
        static void Main(string[] args)
        {
            //UserService.CreateUser("Pubey", "addd@gmail.com");
        }

        private static User CreateUser()
        {
            return new User()
            {
                UserName = "Miso"
            };
        }

        private static Brewery CreateBrewery()
        {
            return new Brewery()
            {
                Name = "Starobrno",
                Address = "Mendlak",
                AverageRating = 1.5f
            };
        }
    }
}
