namespace BeerRecommender.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository() {
            context = new AppDbContext();
            entities = context.Users;
        }
    }
}
