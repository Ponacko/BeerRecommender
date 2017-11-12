using BeerRecommender.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerRecommender.Tests
{
    public class ObjectFactory
    {
        private Random Rand = new Random();

        public Beer CreateNewBeer(string name = null, Brewery brewery = null, string category = null, string imageUrl = null, List<UserRating> ratings = null)
        {
            var beer = new Beer
            {
                Id = Rand.Next(),
                Name = name ?? Guid.NewGuid().ToString(),
                ImageUrl = imageUrl,
                Category = category ?? "světlý ležák",
                Brewery = brewery,
                AverageRating = Helper.GetRandomRating(Rand),
                UserRatings = ratings
            };

            return beer;
        }

        public Brewery CreateNewBrewery(string name = null, string address = null, int year = 1995, string city = null, string imageUrl = null)
        {
            Random rnd = new Random();

            var brewery = new Brewery
            {
                Id = Rand.Next(),
                Name = name ?? Guid.NewGuid().ToString(),
                Address = address ?? "someplace",
                YearOfFoundation = year,
                City = city ?? "Sebes",
                ImageUrl = imageUrl
            };

            return brewery;
        }

        public User CreateNewUser(string userName = null, ICollection<UserRating> ratings = null)
        {
            var user = new User
            {
                Id = Rand.Next(),
                UserName = userName ?? Guid.NewGuid().ToString(),
                UserRatings = ratings
            };

            return user;
        }

        public UserRating CreateNewUserRating(User user, Beer beer, float rating = 2.5f, bool isPrediction = false)
        {
            var UserRating = new UserRating
            {
                Id = Rand.Next(),
                Beer = beer,
                User = user,
                Rating = rating,
                IsPrediction = isPrediction
            };

            return UserRating;
        }

        public ICollection<User> GenerateUsers(int numberOfUsers)
        {
            var objects = new List<User>();
            for (int i = 0; i < numberOfUsers; ++i) { objects.Add(CreateNewUser()); }
            return objects;
        }

        public ICollection<Brewery> GenerateBreweries(int numberOfUsers)
        {
            var objects = new List<Brewery>();
            for (int i = 0; i < numberOfUsers; ++i) { objects.Add(CreateNewBrewery()); }
            return objects;
        }

        public ICollection<Beer> GenerateBeers(int numberOfUsers, List<Brewery> breweries = null)
        {
            var objects = new List<Beer>();
            for (int i = 0; i < numberOfUsers; ++i)
            {
                objects.Add(CreateNewBeer(brewery: breweries[i % breweries.Count]));
            }
            return objects;
        }

        public ICollection<UserRating> GenerateUserRatings(List<Beer> beers, List<User> users) 
        {
            if (beers.Count != users.Count)
            {
                throw new ArgumentException("Number of beers does not equal number of users");
            }

            var objects = new List<UserRating>();
            for (int i = 0; i < beers.Count; ++i)
            {
                objects.Add(CreateNewUserRating(users[i], beers[i], Helper.GetRandomRating(Rand)));
            }
            return objects;
        }
    }
}
