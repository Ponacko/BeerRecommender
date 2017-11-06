using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeerRecommender.Repositories;
using NUnit.Framework;

namespace BeerRecommender.Tests
{
    [TestFixture]
    public class RepositoryTests
    {
        [Test]
        public void CreateAndRetrieveBrewery()
        {
            var createdBrewery = CreateBrewery();
            var repository = new BreweryRepository();
            var insertedId = repository.Create(createdBrewery);
            var retrievedBrewery = repository.RetrieveById(insertedId);
            Assert.AreEqual(createdBrewery, retrievedBrewery);
        }

        [Test]
        public void UpdateBrewery() {
            var createdBrewery = CreateBrewery();
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
            var createdBrewery = CreateBrewery();
            var repository = new BreweryRepository();
            var insertedId = repository.Create(createdBrewery);
            repository.Delete(insertedId);
            var brewery = repository.RetrieveById(insertedId);

            Assert.IsNull(brewery);
        }

        [Test]
        public void CreateAndRetrieveUser()
        {
            var createdUser = CreateUser();
            var repository = new UserRepository();
            var insertedId = repository.Create(createdUser);
            var retrievedUser = repository.RetrieveById(insertedId);
            Assert.AreEqual(createdUser, retrievedUser);
        }

        [Test]
        public void UpdateUser()
        {
            var createdUser = CreateUser();
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
            var createdUser = CreateUser();
            var repository = new UserRepository();
            var insertedId = repository.Create(createdUser);
            repository.Delete(insertedId);
            var user = repository.RetrieveById(insertedId);

            Assert.IsNull(user);
        }

        [Test]
        public void CreateAndRetrieveRating()
        {
            var createdUser = CreateUser();
            var createdBrewery = CreateBrewery();
            var createdRating = CreateUserRating(createdUser, createdBrewery);
            var repository = new UserRatingRepository();
            var insertedId = repository.Create(createdRating);
            var retrievedRating = repository.RetrieveById(insertedId);
            Assert.AreEqual(createdRating, retrievedRating);
        }

        [Test]
        public void UpdateRating()
        {
            var createdUser = CreateUser();
            var createdBrewery = CreateBrewery();
            var createdRating = CreateUserRating(createdUser, createdBrewery);
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
            var createdUser = CreateUser();
            var createdBrewery = CreateBrewery();
            var createdRating = CreateUserRating(createdUser, createdBrewery);
            var repository = new UserRatingRepository();
            var insertedId = repository.Create(createdRating);
            repository.Delete(insertedId);
            var rating = repository.RetrieveById(insertedId);

            Assert.IsNull(rating);
        }


        private static Brewery CreateBrewery()
        {
            return new Brewery()
            {
                Name = "test",
                Address = "test",
                AverageRating = 1.5f
            };
        }

        private static User CreateUser() {
            return new User() {
                UserName = "username"
            };
        }

        private static UserRating CreateUserRating(User user, Brewery brewery) {
            return new UserRating() {
                User = user,
                Brewery = brewery,
                Rating = 0.5f
            };
        }
    }
}
