using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeerRecommender
{
    public class Brewery
    {
        public Brewery() { }

        public int BreweryID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public float AverageRating { get; set; }
        public virtual ICollection<UserRating> UserRatings { get; set; }

        public override bool Equals(object obj) {
            var other = obj as Brewery;
            if (other == null)
                return false;
            return Name.Equals(other.Name) && Address.Equals(other.Address);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override string ToString() {
            return $"Brewery {Name} at address {Address} with rating {AverageRating}";
        }
    }
}
