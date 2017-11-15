using System;
using System.Data.Entity.Validation;
using System.Linq;
using BeerRecommender.Repositories;
using BL.Services;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using BeerRecommender.Utils;

namespace BeerRecommender.Tests
{
    [TestFixture]
    public class ServiceTests
    {
        ObjectFactory Factory = new ObjectFactory();

        private readonly Random rand = new Random();

        [Test]
        public void AddRatingTest()
        {
            var createdUser = Factory.CreateNewUser();
            var createdBeer = Factory.CreateNewBeer("Starobrno");
            var urr = new UserRatingRepository();
            var ur = new UserRepository();
            var br = new BeerRepository();
            //ur.Create(createdUser);
            br.Create(createdBeer);

            var createdId = RatingService.AddRating(createdUser, createdBeer, 4.7f);
            var retrieved = urr.RetrieveById(createdId);
            Assert.AreEqual(retrieved.Id, createdId);
        }

        [Test]
        public void CalculateSimilarityTest() {
            var user1 = Factory.CreateNewUser();
            var user2 = Factory.CreateNewUser();
            var beers = Factory.GenerateBeers(5);
            var repository = new UserRatingRepository();
            foreach (var beer in beers) {
                var randomRating = Helper.GetRandomRating(rand);
                var rating1 = new UserRating() {
                    User = user1,
                    Beer = beer,
                    Rating = randomRating,
                };
                var rating2 = new UserRating()
                {
                    User = user2,
                    Beer = beer,
                    Rating = 6 - randomRating,
                };
                
                repository.Create(rating1);
                repository.Create(rating2);
            }
            var similarity = SimilarityService.CalculateSimilarity(user1, user2);
            Assert.AreEqual(-1, similarity, 0.00001d);
        }

        [Test, Ignore("Duplicite key")]
        public void AddSimilarityTest() {
            var user1 = Factory.CreateNewUser();
            var user2 = Factory.CreateNewUser();
            var repository = new UserRatingRepository();
            var beers = Factory.GenerateBeers(5);
            foreach (var beer in beers)
            {
                var randomRating = Helper.GetRandomRating(rand);
                var rating1 = new UserRating()
                {
                    User = user1,
                    Beer = beer,
                    Rating = randomRating,
                };
                var rating2 = new UserRating()
                {
                    User = user2,
                    Beer = beer,
                    Rating = 6 - randomRating,
                };

                repository.Create(rating1);
                repository.Create(rating2);
            }
            var createdId = SimilarityService.AddSimilarity(user1, user2);
            Assert.NotNull(repository.RetrieveById(createdId));
        }
    }
}
