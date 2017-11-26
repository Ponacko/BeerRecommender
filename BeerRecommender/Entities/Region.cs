using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerRecommender.Entities
{
    public class Region : Entity
    {
        public Region() {
            Breweries = new List<Brewery>();
            Users = new List<User>();
        }
        public string Name { get; set; }
        public List<Brewery> Breweries { get; set; }
        public List<User> Users { get; set; }
    }
}
