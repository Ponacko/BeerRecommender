namespace BeerRecommender
{
    public class UserRating
    {
        public UserRating() {}

        public int UserRatingId { get; set; }
        public User User { get; set; }
        public Brewery Brewery { get; set; }
    }
}
