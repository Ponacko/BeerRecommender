using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeerRecommender;
using BeerRecommender.Repositories;

namespace BL.Services
{
    public class RecommendationService
    {
        public IEnumerable<Beer> GetMostPopularBeers(int numberOfBeers) {
            var repository = new BeerRepository();
            var beers = repository.RetrieveAll();
            beers.ForEach(UpdateAverageRating);
            return beers.OrderByDescending(b => b.AverageRating).Take(numberOfBeers);
        }

        public void UpdateAverageRating(Beer beer) {
            var repository = new BeerRepository();
            beer.AverageRating = beer.UserRatings.Average(ur => ur.Rating);
            repository.Update(beer);
        }

        // TODO: personalized recommendations based on most similar users
    }
}
