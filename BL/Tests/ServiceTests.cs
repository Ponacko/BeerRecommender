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
        ObjectFactory Factory = new ObjectFactory();

        [Test]
        public void AddRatingTest()
        {
            var createdUser = Factory.CreateNewUser(userName: "Miso");
            var createdBeer = Factory.CreateNewBeer("Starobrno");
            var urr = new UserRatingRepository();
            var br = new BeerRepository();
            var ur = new UserRepository();
            ur.Create(createdUser);

            var createdId = RatingService.AddRating(createdUser, createdBeer, 4.7f);
            var retrieved = urr.RetrieveById(createdId);
            Assert.AreEqual(retrieved.Id, createdId);
        }
    }
}
