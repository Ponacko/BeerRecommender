using System.Collections.Generic;

namespace BeerRecommender {
    public class User {
        public User() {}

        public int UserId { get; set; }
        public string UserName { get; set; }
        public virtual ICollection<UserRating> UserRatings { get; set; }
    }
}
