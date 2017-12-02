using BeerRecommender.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerRecommender.Repositories
{
    public class BeerRepository : Repository<Beer>
    {
        public BeerRepository()
        {
            context = new AppDbContext();
            entities = context.Beers;
        }

        public List<Beer> RetrieveBeersByTag(Tag tag)
        {
            var retrievedTags = context.Beers.Include(x => x.Tags)
                .ToList()
                .Where(b => b.Tags.Contains(tag));
            return retrievedTags.ToList();
        }

        public List<Tag> RetrieveTagsFromBeers(List<Beer> beers)
        {
            var beersList = context.Beers.Include(x => x.Tags)
                .ToList();
            var y = beersList.Where(b => beers.Contains(b))
                .ToList();

            var tagsList = beersList.SelectMany(x => x.Tags);
            var tagsSet = new HashSet<Tag>(tagsList);  
              
            return tagsSet.ToList();
        }
    }
}
