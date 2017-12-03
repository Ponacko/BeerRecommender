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
        public string Abbreviation { get; set; }
        public List<Brewery> Breweries { get; set; }
        public List<User> Users { get; set; }

        public static bool operator ==(Region obj1, Region obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }
            return obj1.Id == obj2.Id;
        }

        public static bool operator !=(Region obj1, Region obj2)
        {
            return !(obj1 == obj2);
        }

        public override bool Equals(object region)
        {
            var item = region as Region;

            if (item == null)
            {
                return false;
            }

            return Id.Equals(item.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
