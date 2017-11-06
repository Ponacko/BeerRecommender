using BeerRecommender.Entities;

namespace BeerRecommender
{
    public class UserRating : Entity
    {
        public User User { get; set; }
        public Brewery Brewery { get; set; }
        public float Rating { get; set; }
        public bool IsPrediction { get; set; }
    }
}
