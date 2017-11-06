namespace BeerRecommender.Repositories
{
    public class UserRatingRepository : Repository<UserRating>
    {
        public UserRatingRepository() {
            using (var context = new AppDbContext()) {
                entities = context.UserRatings;
            }
        }
    }
}
