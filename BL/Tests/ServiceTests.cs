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
    }
}
