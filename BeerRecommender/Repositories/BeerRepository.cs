using System;
using System.Collections.Generic;
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
    }
}
