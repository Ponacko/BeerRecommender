using BeerRecommender.Repositories;
using NUnit.Framework;

namespace BeerRecommender.Tests
{
    [TestFixture]
    public class RepositoryTests
    {
        ObjectFactory Factory = new ObjectFactory();

        [Test]
        public void CreateAndRetrieveBrewery()
        {
            var createdBrewery = Factory.CreateNewBrewery();
            var repository = new BreweryRepository();
            var insertedId = repository.Create(createdBrewery);
            var retrievedBrewery = repository.RetrieveById(insertedId);
            Assert.AreEqual(createdBrewery, retrievedBrewery);
        }

        [Test]
        public void UpdateBrewery() {
            var createdBrewery = Factory.CreateNewBrewery();
            var repository = new BreweryRepository();
            var insertedId = repository.Create(createdBrewery);
            const string newAddress = "test2";
            createdBrewery.Address = newAddress;
            repository.Update(createdBrewery);

            var retrievedBrewery = repository.RetrieveById(insertedId);
            Assert.AreEqual(newAddress, retrievedBrewery.Address);
        }

        [Test]
        public void DeleteBrewery() {
            var createdBrewery = Factory.CreateNewBrewery();
            var repository = new BreweryRepository();
            var insertedId = repository.Create(createdBrewery);
            repository.Delete(insertedId);
            var brewery = repository.RetrieveById(insertedId);

            Assert.IsNull(brewery);
        }

        [Test]
        public void CreateAndRetrieveUser()
        {
            var createdUser = Factory.CreateNewUser();
            var repository = new UserRepository();
            var insertedId = repository.Create(createdUser);
            var retrievedUser = repository.RetrieveById(insertedId);
            Assert.AreEqual(createdUser, retrievedUser);
        }

        [Test]
        public void UpdateUser()
        {
            var createdUser = Factory.CreateNewUser();
            var repository = new UserRepository();
            var insertedId = repository.Create(createdUser);
            const string newName = "test2";
            createdUser.UserName = newName;
            repository.Update(createdUser);

            var retrievedUser = repository.RetrieveById(insertedId);
            Assert.AreEqual(newName, retrievedUser.UserName);
        }

        [Test]
        public void DeleteUser()
        {
            var createdUser = Factory.CreateNewUser();
            var repository = new UserRepository();
            var insertedId = repository.Create(createdUser);
            repository.Delete(insertedId);
            var user = repository.RetrieveById(insertedId);

            Assert.IsNull(user);
        }

        [Test]
        public void CreateAndRetrieveRating()
        {
            var createdUser = Factory.CreateNewUser();
            var createdBeer = Factory.CreateNewBeer();
            var createdRating = Factory.CreateNewUserRating(createdUser, createdBeer);
            var repository = new UserRatingRepository();
            var insertedId = repository.Create(createdRating);
            var retrievedRating = repository.RetrieveById(insertedId);
            Assert.AreEqual(createdRating, retrievedRating);
        }

        [Test]
        public void UpdateRating()
        {
            var createdUser = Factory.CreateNewUser();
            var createdBeer = Factory.CreateNewBeer();
            var createdRating = Factory.CreateNewUserRating(createdUser, createdBeer);
            var repository = new UserRatingRepository();
            var insertedId = repository.Create(createdRating);
            var newRating = 5f;
            createdRating.Rating = newRating;
            repository.Update(createdRating);

            var retrievedRating = repository.RetrieveById(insertedId);
            Assert.AreEqual(newRating, retrievedRating.Rating);
        }

        [Test]
        public void DeleteRating()
        {
            var createdUser = Factory.CreateNewUser();
            var createdBeer = Factory.CreateNewBeer();
            var createdRating = Factory.CreateNewUserRating(createdUser, createdBeer);
            var repository = new UserRatingRepository();
            var insertedId = repository.Create(createdRating);
            repository.Delete(insertedId);
            var rating = repository.RetrieveById(insertedId);

            Assert.IsNull(rating);
        }
    }
}
