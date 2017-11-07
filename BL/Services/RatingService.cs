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
        private static BreweryRepository breweryRepository = new BreweryRepository();
        private static UserRatingRepository userRatingRepository = new UserRatingRepository();
        private static UserRepository userRepository = new UserRepository();

        public static int AddRating(User user, Brewery brewery, float rating)
        {
            var userRating = new UserRating()
            {
                User = user,
                Brewery = brewery,
                Rating = rating,
                IsPrediction = false
            };
            int ratingId = userRatingRepository.Create(userRating);

            brewery.UserRatings.Add(userRating);
            breweryRepository.Update(brewery);

            user.UserRatings.Add(userRating);
            userRepository.Update(user);

            return ratingId;
        }
    }
}
