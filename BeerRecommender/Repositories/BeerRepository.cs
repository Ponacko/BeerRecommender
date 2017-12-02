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

        public List<Beer> RetrieveBeerByTag(Tag tag)
        {
            var retrievedTags = context.Beers.ToList().Where(b => b.Tags.Contains(tag));
            var piva = context.Beers.ToList().Take(20);
            var kokot = retrievedTags.Count();
            var pica = retrievedTags.Take(20);
            return retrievedTags.ToList();
        }
    }
}
