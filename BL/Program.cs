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
            RatingService.AddRating(CreateUser(), CreateBrewery(), 1.0f);
            using (var context = new AppDbContext())
            {
                foreach (var rating in context.UserRatings.OrderBy(b => b.Rating).Distinct())
                {
                    Console.WriteLine(rating);
                }
            }
            Console.ReadLine();  
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
