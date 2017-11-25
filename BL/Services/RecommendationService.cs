using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeerRecommender;
using BeerRecommender.Repositories;

namespace BL.Services
{
    public class RecommendationService
    {
        public static IEnumerable<Beer> GetMostPopularBeers(int numberOfBeers) {
            var repository = new BeerRepository();
            var beers = repository.RetrieveAll();
            return beers.OrderByDescending(b => b.AverageRating).Take(numberOfBeers);
        }

        // TODO: personalized recommendations based on most similar users
    }
}
