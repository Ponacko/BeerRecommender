using BeerRecommender;
using BeerRecommender.Entities;
using BeerRecommender.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class RatingService
    {
        private static BeerRepository beerRepository = new BeerRepository();
        private static UserRatingRepository userRatingRepository = new UserRatingRepository();
        private static UserRepository userRepository = new UserRepository();

        public static int AddRating(User user, Beer beer, float rating)
        {
            var userRating = new UserRating()
            {
                User = user,
                Beer = beer,
                Rating = rating,
                IsPrediction = false
            };
            
            int ratingId = userRatingRepository.Create(userRating);

            beer.UserRatings.Add(userRating);
            beerRepository.Update(beer);
            beer.AverageRating = beer.UserRatings.Select(x => x.Rating).Average();

            user.UserRatings.Add(userRating);
            userRepository.Update(user);

            return ratingId;
        }
    }
}
