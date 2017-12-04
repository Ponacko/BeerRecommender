using BeerRecommender;
using BeerRecommender.Repositories;
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
            BeerRepository br = new BeerRepository();
            TagRepository tr = new TagRepository();
            RegionRepository rr = new RegionRepository();
            var region = rr.RetrieveAll().Take(1).First();
            var popularBeers = BeerService.GetPopularBeers().Take(3).ToList();

            Console.WriteLine("Your picked beers were:");
            popularBeers.ForEach(b => Console.WriteLine(b.Name));

            Console.WriteLine();
            Console.WriteLine("Our recommendation is:");
            var finalyDone = RecommendationService.Recommend(popularBeers, 10/*, region*/);
            finalyDone.ForEach(x => Console.WriteLine(x.Name));
            Console.ReadKey();
        }

        private static Brewery CreateBrewery()
        {
            return new Brewery()
            {
                Name = "Starobrno",
                Address = "Mendlak",
            };
        }
    }
}
