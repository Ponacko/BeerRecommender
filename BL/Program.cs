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
            //UserService.CreateUser("Pubey", 18, "addd@gmail.com");
            BeerRepository br = new BeerRepository();
            TagRepository tr = new TagRepository();
            /*List<Beer> beers = br.RetrieveBeerByTag(
                tr.RetrieveTagByName("světlé"));
            beers?.ForEach(x => Console.WriteLine(x.Name));
            var x = TagService.GetTagsFromBeers(beers);
            foreach(var a in x)
            {
                Console.WriteLine(a.Key.Name);
                Console.WriteLine(a.Value);
            }
            Console.ReadLine();*/
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
