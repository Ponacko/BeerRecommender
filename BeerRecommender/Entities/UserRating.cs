namespace BeerRecommender
{
    public class UserRating
    {
        public int UserRatingId { get; set; }
        public User User { get; set; }
        public Brewery Brewery { get; set; }
        public float Rating { get; set; }
        public bool IsPrediction { get; set; }
    }
}
