using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BeerRecommender.Entities;

namespace BeerRecommender
{
    public class Brewery : Entity
    {
        public Brewery() {
            UserRatings = new List<UserRating>();
        }
        
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public float AverageRating { get; set; }
        public virtual ICollection<UserRating> UserRatings { get; set; }
        
        public override string ToString() {
            return $"Brewery {Name} at address {Address} with rating {AverageRating}";
        }
    }
}
