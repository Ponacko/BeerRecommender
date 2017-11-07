using System.Collections.Generic;
using BeerRecommender.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeerRecommender
{
    public class User : Entity
    {
        public User()
        {
            UserRatings = new List<UserRating>();
        }

        [Index(IsUnique = true)]
        [StringLength(100)]
        public string UserName { get; set; }

        [Index(IsUnique = true)]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public virtual ICollection<UserRating> UserRatings { get; set; }
    }
}
