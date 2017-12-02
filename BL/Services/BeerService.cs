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

        public static List<Beer> GetBeersByIds(List<string> ids) {
            BeerRepository br = new BeerRepository();
            List<Beer> list = new List<Beer>();
            foreach (var id in ids) {
                var beerId = int.Parse(id);
                list.Add(br.RetrieveById(beerId));
            }

            return list;
        }
    }
}
