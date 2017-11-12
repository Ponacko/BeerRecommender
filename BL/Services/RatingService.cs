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
        private static readonly BeerRepository BeerRepository = new BeerRepository();
        private static readonly UserRatingRepository UserRatingRepository = new UserRatingRepository();
        private static readonly UserRepository UserRepository = new UserRepository();

        public static int AddRating(User user, Beer beer, float rating)
        {
            var userRating = new UserRating()
            {
                User = user,
                Beer = beer,
                Rating = rating,
                IsPrediction = false
            };
            
            var ratingId = UserRatingRepository.Create(userRating);

            beer.UserRatings.Add(userRating);
            BeerRepository.Update(beer);
            beer.AverageRating = beer.UserRatings.Select(x => x.Rating).Average();

            user.UserRatings.Add(userRating);
            UserRepository.Update(user);

            return ratingId;
        }
    }
}
