using System.Collections.Generic;
using System.Linq;
using BeerRecommender.Entities;

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

        public List<Beer> GetUserPickedBeers(int userId) {
            return entities.Where(u => u.Id == userId).SelectMany(u => u.PickedBeers).Distinct().ToList();
        }

        public void AddRecommendedBeer(User user, Beer beer)
        {
            //context.Beers.Attach(beer);
            user.RecommendedBeers.Add(beer);
        }

        public List<Beer> GetUserRecommendedBeers(int userId)
        {
            return entities.Where(u => u.Id == userId).SelectMany(u => u.RecommendedBeers).Distinct().ToList();
        }

        public void SetUserRegion(User user, Region region)
        {
            context.Regions.Attach(region);
            user.Region = region;
        }

        public Region GetUserRegion(int userId) {
            return entities.Where(u => u.Id == userId).Select(u => u.Region).First();
        }
    }
}
