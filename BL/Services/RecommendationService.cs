using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeerRecommender;
using BeerRecommender.Repositories;
using System;
using BeerRecommender.Entities;

namespace BL.Services {
    public class RecommendationService {
        public static BeerRepository Repository => new BeerRepository();

        public static List<Beer> RecommendRandomBeers(int numberOfBeers) {
            var beers = Repository.RetrieveAll();
            var random = new Random();
            var randomBeers = new List<Beer>();
            for (int i = 0; i < numberOfBeers; i++) {
                var randomBeerId = random.Next(0, beers.Count);
                randomBeers.Add(Repository.RetrieveById(randomBeerId));
            }
            return randomBeers;
        }

        public static List<Beer> ReccomendBeersSingle(List<Beer> pickedPopularBeers, Region selectedRegion = null) {
            var allBeers = GetAllBeersForRegion(pickedPopularBeers, selectedRegion).ToList();
            var recommendedBeers = new List<Beer>();
            foreach (var beer in pickedPopularBeers) {
                var pickedTags = Repository.RetrieveTagsFromBeers(new List<Beer> {beer});
                var beersContainingSelectedTags = GetBeersContainingSelectedTags(allBeers, pickedTags);
                recommendedBeers.Add(
                    GetBeersWithBestCoefficient(1, beersContainingSelectedTags, pickedTags.ToDictionary(t => t, t => 1)
                    ).First());
            }
            return recommendedBeers;
        }

        private static List<Beer> GetBeersContainingSelectedTags(IEnumerable<Beer> allBeers, List<Tag> pickedTags) {
            return allBeers
                .Where(b => b.Tags.Intersect(pickedTags).Any())
                .ToList();
        }

        public static List<Beer> Recommend(List<Beer> pickedPopularBeers, int numberOfBeersToRecommend,
            Region selectedRegion = null) {
            var tagsFromPickedPopularBeers = Repository.RetrieveTagsFromBeers(pickedPopularBeers);

            Dictionary<Tag, int> tagWeightDictionary;
            List<Tag> tagsFromDictionary;
            GetGroupedTags(tagsFromPickedPopularBeers, out tagWeightDictionary, out tagsFromDictionary);


            // Beers containing tags
            var allBeers = GetAllBeersForRegion(pickedPopularBeers, selectedRegion);
            var beersContainingSelectedTags = GetBeersContainingSelectedTags(allBeers, tagsFromDictionary);

            return GetBeersWithBestCoefficient(numberOfBeersToRecommend, beersContainingSelectedTags,
                tagWeightDictionary);
        }

        private static IEnumerable<Beer> GetAllBeersForRegion(List<Beer> pickedPopularBeers, Region selectedRegion) {
            var allBeers = Repository.RetrieveAllBeersWithBreweriesAndTags().Except(pickedPopularBeers);
            if (selectedRegion != null) {
                allBeers = allBeers.Where(b => b.Brewery?.Region == selectedRegion).ToList();
            }
            return allBeers;
        }

        private static List<Beer> GetBeersWithBestCoefficient(int numberOfBeersToRecommend,
            List<Beer> beersContainingSelectedTags,
            Dictionary<Tag, int> tagWeightDictionary) {
            var beersWithCoefficient = CalculateCoefficients(beersContainingSelectedTags, tagWeightDictionary);
            var sortedBeersByCoefficient = beersWithCoefficient.OrderByDescending(b => b.Item2).ToList();

            return sortedBeersByCoefficient.Take(numberOfBeersToRecommend).Select(r => r.Item1).ToList();
        }

        private static void GetGroupedTags(List<Tag> tagsFromPickedPopularBeers,
            out Dictionary<Tag, int> tagWeightDictionary, out List<Tag> tagsFromDictionary) {
            var groups = tagsFromPickedPopularBeers.GroupBy(s => s)
                .Select(s => new {Tag = s.Key, Count = s.Count()});
            tagWeightDictionary = groups.ToDictionary(g => g.Tag, g => g.Count);
            tagsFromDictionary = tagWeightDictionary.Keys.ToList();
        }

        private static List<Tuple<Beer, int>> CalculateCoefficients(List<Beer> beersContainingSelectedTags,
            Dictionary<Tag, int> tagWeightDictionary) {
            var beersWithCoefficient = new List<Tuple<Beer, int>>();
            foreach (var b in beersContainingSelectedTags) {
                int score = 0;
                var a = b.Tags
                    .Where(r => tagWeightDictionary.Keys.Contains(r))
                    .ToList();
                foreach (var tag in a) {
                    int value = 0;
                    tagWeightDictionary.TryGetValue(tag, out value);
                    score += value;
                }
                beersWithCoefficient.Add(new Tuple<Beer, int>(b, score));
            }
            return beersWithCoefficient;
        }
    }
}