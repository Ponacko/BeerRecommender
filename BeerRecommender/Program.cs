using System;
using System.Linq;

namespace BeerRecommender
{
    class Program
    {
        static void Main(string[] args) {
            using (var context = new AppDbContext())
            {
                //PrintBreweriesFromDb(context);
                //PrintBeersFromDb(context);
                PrintNamesOfPopularBeers(context);
            }
            Console.ReadLine();
        }

        private static void PrintNamesOfPopularBeers(AppDbContext context)
        {
            Console.WriteLine("Printing the selected 20 popular beers:");
            foreach (var beer in context.Beers.Where(b => b.IsPopular).ToList())
            {
                Console.WriteLine(beer.Name);
            }
        }

        private static void PrintBreweriesFromDb(AppDbContext context)
        {
            foreach (var brewery in context.Breweries.OrderBy(b => b.Name).Distinct())
            {
                Console.WriteLine(brewery);
            }
        }

        private static void PrintBeersFromDb(AppDbContext context)
        {
            foreach (var beer in context.Beers.OrderBy(b => b.Name).Distinct())
            {
                Console.WriteLine(beer);
            }
        }
    }
}
