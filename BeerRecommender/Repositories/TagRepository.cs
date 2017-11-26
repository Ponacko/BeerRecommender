using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeerRecommender.Entities;

namespace BeerRecommender.Repositories
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository() {
            context = new AppDbContext();
            entities = context.Tags;
        }
    }
}
