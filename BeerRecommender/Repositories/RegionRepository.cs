using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeerRecommender.Entities;

namespace BeerRecommender.Repositories
{
    public class RegionRepository : Repository<Region>
    {
        public RegionRepository() {
            context = new AppDbContext();
            entities = context.Regions;
        }
    }
}
