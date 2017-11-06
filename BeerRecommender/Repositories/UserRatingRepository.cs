namespace BeerRecommender.Repositories
{
    public class UserRatingRepository : Repository<UserRating>
    {
        public UserRatingRepository() {
            context = new AppDbContext();
            entities = context.UserRatings;
        }
    }
}
