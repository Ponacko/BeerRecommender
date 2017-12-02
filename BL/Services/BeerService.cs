using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeerRecommender;
using BeerRecommender.Repositories;
using BeerRecommender.Entities;

namespace BL.Services
{
    public class BeerService
    {
        public static List<Beer> GetAllBeers()
        {
            var repository = new BeerRepository();
            return repository.RetrieveAll();
        }

        public static List<Beer> GetPopularBeers()
        {
            return GetAllBeers().FindAll(b => b.IsPopular).ToList();
        }

        public static List<Beer> RecommendRandomBeers(int numberOfBeers)
        {
            BeerRepository br = new BeerRepository();
            var beers = br.RetrieveAll();
            var random = new Random();
            var randomBeers = new List<Beer>();
            for (int i = 0; i < numberOfBeers; i++)
            {
                var randomBeerId = random.Next(0, beers.Count);
                randomBeers.Add(br.RetrieveById(randomBeerId));
            }
            return randomBeers;
        }

        public static HashSet<Beer> GetBeersContainingTags(List<Tag> tags)
        {
            BeerRepository br = new BeerRepository();
            var beers = br.RetrieveAll();
            HashSet<Beer> foundBeers = new HashSet<Beer>();
            foreach(var tag in tags)
            {
                foundBeers.UnionWith(br.RetrieveBeersByTag(tag));
            }
            return foundBeers;
        }
    }
}
