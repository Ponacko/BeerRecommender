using System.Collections.Generic;
using BeerRecommender.Entities;

namespace BeerRecommender {
    public class User : Entity {
        public User() {
            UserRatings = new List<UserRating>();
        }
        
        public string UserName { get; set; }
        public virtual ICollection<UserRating> UserRatings { get; set; }
    }
}
