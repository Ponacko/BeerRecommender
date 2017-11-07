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
        [Required(ErrorMessage = "Invalid Username")]
        public string UserName { get; set; }

        [Index(IsUnique = true)]
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [Range(18, 200)]
        public int Age { get; set; }

        public virtual ICollection<UserRating> UserRatings { get; set; }
    }
}
