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
            var beersWithTags = beersList.Where(b => beers.Contains(b))
                .ToList();

            var tagsList = beersWithTags.SelectMany(x => x.Tags);
              
            return tagsList.ToList();
        }

        public List<Beer> RetrieveAllBeersWithBreweriesAndTags()
        {
            return context.Beers.Include(b => b.Tags).Include(b => b.Brewery).Include(b => b.Brewery.Region).ToList();
        }

        public AppDbContext Context
        {
            get { return context; }
            set { context = value; }
        }
    }
}
