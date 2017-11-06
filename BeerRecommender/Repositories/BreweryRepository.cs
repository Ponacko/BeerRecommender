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
    }
}
