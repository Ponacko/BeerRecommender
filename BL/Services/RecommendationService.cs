using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeerRecommender;
using BeerRecommender.Repositories;
using System;
using BeerRecommender.Entities;

namespace BL.Services
{
    public class RecommendationService
    {
        public static List<Beer> RecommendRandomBeers(int numberOfBeers)
        {
            BeerRepository br = new BeerRepository();
            var beers = br.RetrieveAll();
            var random = new Random();
            var randomBeers = new List<Beer>();
            for (int i = 0; i < numberOfBeers; i++)
            {
                var randomBeerId = random.Next(0, beers.Count);
                randomBeers.Add(br.RetrieveById(randomBeerId));
            }
            return randomBeers;
        }

        public static List<Beer> AssignValuesToBeersForRecommendation(
            Dictionary<Tag, int> tagsWithValues, 
            List<Beer> beers,
            int numberOfBeersToRecommend)
        {
            var beersWithCoefficient = new Dictionary<Beer, int>();
            foreach(var b in beers)
            {
                int score = 0;
                var a = b.Tags
                    .Where(r => tagsWithValues.Keys.Contains(r))
                    .ToList();
                foreach(var tag in a)
                {
                    int value = 0;
                    tagsWithValues.TryGetValue(tag, out value);
                    score += value;
                }
                beersWithCoefficient.Add(b, score);
            }
            return beersWithCoefficient.Keys
                .Take(numberOfBeersToRecommend)
                .ToList();
        }      
    }
}
