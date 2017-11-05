using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace BeerRecommender
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new BeerDbContext())
            {
                PrintBreweriesFromDb(context);
            }
            Console.ReadLine();
        }

        private static void PrintBreweriesFromDb(BeerDbContext context)
        {
            foreach (var brewery in context.Breweries.OrderBy(b => b.Name).Distinct())
            {
                Console.WriteLine(brewery);
            }
        }
    }
}
