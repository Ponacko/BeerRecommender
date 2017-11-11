using BeerRecommender.Entities;
using System.ComponentModel.DataAnnotations;

namespace BeerRecommender
{
    public class UserRating : Entity
    {
        [Required]
        public User User { get; set; }

        [Required]
        public Beer Beer { get; set; }

        [Required]
        [Range(1.00f,5.00f)]
        public float Rating { get; set; }

        public bool IsPrediction { get; set; } = false;

        public override string ToString()
        {
            return $"User {User}rates {Beer}with rating {Rating}.";
        }
    }
}
