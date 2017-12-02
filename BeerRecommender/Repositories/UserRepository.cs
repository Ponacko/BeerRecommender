using System.Linq;

namespace BeerRecommender.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository() {
            context = new AppDbContext();
            entities = context.Users;
        }

        public void AddPickedBeer(User user, Beer beer) {
            context.Beers.Attach(beer);
            user.PickedBeers.Add(beer);
        }
    }
}
