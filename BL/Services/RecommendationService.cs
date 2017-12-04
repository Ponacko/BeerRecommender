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
        public static BeerRepository Repository =>  new BeerRepository();

        public static List<Beer> RecommendRandomBeers(int numberOfBeers)
        {
            var beers = Repository.RetrieveAll();
            var random = new Random();
            var randomBeers = new List<Beer>();
            for (int i = 0; i < numberOfBeers; i++)
            {
                var randomBeerId = random.Next(0, beers.Count);
                randomBeers.Add(Repository.RetrieveById(randomBeerId));
            }
            return randomBeers;
        }

        public static List<Beer> Recommend(List<Beer> pickedPopularBeers, int numberOfBeersToRecommend, Region selectedRegion = null)
        {
            var tagsFromPickedPopularBeers = Repository.RetrieveTagsFromBeers(pickedPopularBeers);

            var groups = tagsFromPickedPopularBeers.GroupBy(s => s)
                .Select(s => new { Tag = s.Key, Count = s.Count() });
            var tagWeightDictionary = groups.ToDictionary(g => g.Tag, g => g.Count);
            var tagsFromDictionary = tagWeightDictionary.Keys.ToList();


            // Beers containing tags
            var allBeers = Repository.RetrieveAllBeersWithBreweries().Except(pickedPopularBeers);
            if (selectedRegion != null)
            {
                allBeers = allBeers.Where(b => b.Brewery?.Region == selectedRegion).ToList();
            }
            var beersContainingSelectedTags = allBeers
                .Where(b => b.Tags.Intersect(tagsFromDictionary).Any())
                .ToList();

            var beersWithCoefficient = new List<Tuple<Beer, int>>();
            foreach (var b in beersContainingSelectedTags)
            {
                int score = 0;
                var a = b.Tags
                    .Where(r => tagWeightDictionary.Keys.Contains(r))
                    .ToList();
                foreach (var tag in a)
                {
                    int value = 0;
                    tagWeightDictionary.TryGetValue(tag, out value);
                    score += value;
                }
                beersWithCoefficient.Add(new Tuple<Beer, int>(b, score));
            }
            var sortedBeersByCoefficient = beersWithCoefficient.OrderByDescending(b => b.Item2).ToList();

            return sortedBeersByCoefficient.Take(numberOfBeersToRecommend).Select(r => r.Item1).ToList();
        }
    }
}
