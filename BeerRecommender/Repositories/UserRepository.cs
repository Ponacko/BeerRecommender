using System.Linq;

namespace BeerRecommender.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository() {
            context = new AppDbContext();
            entities = context.Users;
        }

        public User RetrieveByUserName(string userName)
        {
            return !entities.Any(e => e.UserName == userName) ? null : entities.First(e => e.UserName == userName);
        }
    }
}
