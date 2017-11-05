using System.Collections.Generic;

namespace BeerRecommender {
    public class User {
        public User() {
            UserRatings = new List<UserRating>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public virtual ICollection<UserRating> UserRatings { get; set; }
    }
}
