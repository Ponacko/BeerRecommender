using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeerRecommender;
using BeerRecommender.Entities;
using BeerRecommender.Repositories;
using MathNet.Numerics.Statistics;

namespace BL.Services
{
    public class SimilarityService
    {
        public static double CalculateSimilarity(User user1, User user2) {
            var bias1 = CalculateBias(user1);
            var bias2 = CalculateBias(user2);
            var ratings = user1.UserRatings
                .Where(u => !u.IsPrediction)
                .Join(user2.UserRatings
                    .Where(u => !u.IsPrediction), r1 => r1.Beer, r2 => r2.Beer, 
                    (r1, r2) => new { Rating1 = r1.Rating - bias1, Rating2 = r2.Rating - bias2}).ToList();
            
            return Correlation.Pearson(ratings.Select(r => r.Rating1), ratings.Select(r => r.Rating2));
        }

        public static int AddSimilarity(User user1, User user2) {
            var similarity = new UserSimilarity() {
                User1 = user1,
                User2 = user2,
                Similarity = CalculateSimilarity(user1, user2)
            };
            var repository = new UserSimilarityRepository();
            return repository.Create(similarity);
        }

        public static double CalculateBias(User user) {
            return user.UserRatings.Average(ur => ur.Rating);
        }
    }
}
