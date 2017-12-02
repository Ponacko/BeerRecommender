using BeerRecommender;
using BeerRecommender.Entities;
using BeerRecommender.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public static class TagService
    {
        public static Dictionary<Tag, int> GetTagsFromBeers(List<Beer> beers)
        {
            BeerRepository br = new BeerRepository();
            List<Tag> listOfTags = new List<Tag>();

            foreach(Beer beer in beers){
                listOfTags.AddRange(beer.Tags);
            }

            var groups = listOfTags.GroupBy(s => s)
                .Select(s => new { Tag = s.Key, Count = s.Count() });
            return groups.ToDictionary(g => g.Tag, g => g.Count);
        }
    }
}
