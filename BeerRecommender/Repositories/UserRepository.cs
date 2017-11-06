namespace BeerRecommender.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository() {
            using (var context = new AppDbContext()) {
                entities = context.Users;
            }
        }
    }
}
