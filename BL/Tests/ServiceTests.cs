using BeerRecommender.Repositories;
using BL.Services;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace BeerRecommender.Tests
{
    [TestFixture]
    public class ServiceTests
    {
        [Test]
        public void addRatingTest()
        {
            var createdUser = CreateUser();
            var createdBrewery = CreateBrewery();
            var urr = new UserRatingRepository();
            var br = new BreweryRepository();
            var ur = new UserRepository();
            ur.Create(createdUser);

            var createdId = RatingService.AddRating(createdUser, createdBrewery, 7.7f);
            var retrieved = urr.RetrieveById(createdId);
            Assert.AreEqual(retrieved.Id, createdId);
        }

        private static User CreateUser()
        {
            return new User()
            {
                UserName = "Miso"
            };
        }

        private static Brewery CreateBrewery()
        {
            return new Brewery()
            {
                Name = "Starobrno",
                Address = "Mendlak",
                AverageRating = 1.5f
            };
        }
    }
}
