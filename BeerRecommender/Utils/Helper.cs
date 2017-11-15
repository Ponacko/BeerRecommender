using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerRecommender.Utils
{
    public static class Helper
    {
        public static float GetRandomRating(Random random)
        {
            var rating = random.NextDouble() * 4 + 1;
            rating = Math.Round(rating, 1);

            return (float)rating;
        }
    }
}
