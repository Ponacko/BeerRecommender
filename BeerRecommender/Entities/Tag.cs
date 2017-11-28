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

        public static bool operator ==(Tag obj1, Tag obj2)
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
            return obj1.Name == obj2.Name;
        }

        public static bool operator !=(Tag obj1, Tag obj2)
        {
            return !(obj1.Name == obj2.Name);
        }

        public override bool Equals(object tag)
        {
            var item = tag as Tag;

            if (item == null)
            {
                return false;
            }

            return Name.Equals(item.Name);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}
