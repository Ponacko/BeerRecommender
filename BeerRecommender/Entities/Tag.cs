using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerRecommender.Entities
{
    public class Tag : Entity
    {
        public Tag() {
            Beers = new List<Beer>();
        }

        public List<Beer> Beers { get; set; }
        public string Name { get; set; }
    }
}
