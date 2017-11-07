using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace BeerRecommender.Repositories
{
    public class BreweryRepository : Repository<Brewery> {

        public BreweryRepository() {
            context = new AppDbContext();
            entities = context.Breweries;
        }

        public BreweryRepository(AppDbContext context)
        {
            this.context = context;
            entities = context.Breweries;
        }

        public Brewery RetrieveBreweryByName(string name)
        {
            var retrievedBreweries = context.Breweries.Where(b => b.Name == name);
            return retrievedBreweries.Any() ? retrievedBreweries.First() : null;
        }
    }
}
