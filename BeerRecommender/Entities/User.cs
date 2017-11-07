using System.Collections.Generic;
using BeerRecommender.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeerRecommender {
    public class User : Entity {
        public User() {
            UserRatings = new List<UserRating>();
        }

        [Required]
        public string UserName { get; set; }

        public virtual ICollection<UserRating> UserRatings { get; set; }
    }
}
