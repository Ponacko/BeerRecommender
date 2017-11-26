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
        private readonly Random rand = new Random();

        public User CreateNewUser() {
            return new User();
        }

        public Beer CreateNewBeer(string name = null, Brewery brewery = null, string category = null, string imageUrl = null)
        {
            var beer = new Beer
            {
                Name = name ?? Guid.NewGuid().ToString(),
                ImageUrl = imageUrl,
                Category = category ?? "světlý ležák",
                Brewery = brewery
            };

            return beer;
        }

        public Brewery CreateNewBrewery(string name = null, string address = null, int year = 1995, string city = null, string imageUrl = null)
        {
            var brewery = new Brewery
            {
                Name = name ?? Guid.NewGuid().ToString(),
                Address = address ?? "someplace",
                YearOfFoundation = year,
                City = city ?? "Sebes",
                ImageUrl = imageUrl
            };

            return brewery;
        }
        

        public ICollection<Brewery> GenerateBreweries(int numberOfBreweries)
        {
            var objects = new List<Brewery>();
            for (var i = 0; i < numberOfBreweries; ++i) { objects.Add(CreateNewBrewery()); }
            return objects;
        }

        public ICollection<Beer> GenerateBeers(int numberOfBeers, List<Brewery> breweries = null)
        {
            var objects = new List<Beer>();
            for (var i = 0; i < numberOfBeers; ++i)
            {
                objects.Add(breweries != null ? CreateNewBeer(brewery: breweries[i%breweries.Count]) : CreateNewBeer());
            }
            return objects;
        }
    }
}
