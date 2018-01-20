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

        public static List<Beer> RecommendBeersCombined(List<Beer> pickedPopularBeers, int numberOfRecommendations, Region selectedRegion = null, bool distinctMode = false)
        {
            var allBeers = GetAllBeersForRegion(pickedPopularBeers, selectedRegion).ToList();

            var tagsFromPickedPopularBeers = Repository.RetrieveTagsFromBeers(pickedPopularBeers);
            GetGroupedTags(tagsFromPickedPopularBeers, 
                out Dictionary<Tag, int> tagWeightDictionary, 
                out List<Tag> tagsFromDictionary);
            var beersContainingSelectedTagsMultiple = GetBeersContainingSelectedTags(allBeers, tagsFromDictionary);

            var beersWithCoefficientMult = CalculateCoefficients(beersContainingSelectedTagsMultiple, tagWeightDictionary);
            double coefSumMult = beersWithCoefficientMult.Select(r => r.Item2).Sum();
            var sortedBeersByCoefficientMult = beersWithCoefficientMult.OrderByDescending(b => b.Item2)
                .Select(r => new Tuple<Beer, double>
                (
                    r.Item1,
                    r.Item2 / coefSumMult
                )).ToList();

            var recommendationsWithWeightsMult = sortedBeersByCoefficientMult.Take(50).ToList();

            var unflatennedRecommendationsWithWeights = new List<Tuple<Beer, double>>();
            foreach (var beer in pickedPopularBeers)
            {
                var pickedTags = Repository.RetrieveTagsFromBeers(new List<Beer> { beer });
                var beersContainingSelectedTagsSingle = GetBeersContainingSelectedTags(allBeers, pickedTags);

                var beersWithCoefficient = CalculateCoefficients(beersContainingSelectedTagsSingle, 
                                                                 pickedTags.ToDictionary(t => t, t => 1));
                double coefSum = beersWithCoefficient.Select(r => r.Item2).Sum();
                var sortedBeersByCoefficient = beersWithCoefficient.OrderByDescending(b => b.Item2)
                    .Select(r => new Tuple<Beer, double>
                    (
                        r.Item1,
                        r.Item2 / coefSum
                    )).ToList();

                unflatennedRecommendationsWithWeights.AddRange(sortedBeersByCoefficient.Take(20).ToList());
            }

            unflatennedRecommendationsWithWeights = unflatennedRecommendationsWithWeights.GroupBy(
                b => b.Item1).Select(x => new Tuple<Beer, double>
                (
                    x.Key,
                    x.Max(y => y.Item2)
                )).OrderByDescending(b => b.Item2).ToList();

            unflatennedRecommendationsWithWeights.AddRange(recommendationsWithWeightsMult);
            List<Beer> finalRecommendations;
            if (distinctMode)
            {
                var flatennedRecommendations = unflatennedRecommendationsWithWeights.GroupBy(
                  b => b.Item1).Select(x => new
                  {
                      Beer = x.Key,
                      Weight = x.Sum(y => y.Item2)
                  }).OrderByDescending(b => b.Weight).GroupBy(x => x.Weight).Select(x => new { Weight = x.Key, Beer = x.First().Beer }).ToList();
                finalRecommendations = flatennedRecommendations.Take(numberOfRecommendations).Select(r => r.Beer).ToList();
            } else
            {
                var flatennedRecommendations = unflatennedRecommendationsWithWeights.GroupBy(
                  b => b.Item1).Select(x => new
                  {
                      Beer = x.Key,
                      Weight = x.Sum(y => y.Item2)
                  }).OrderByDescending(b => b.Weight).ToList();
                finalRecommendations = flatennedRecommendations.Take(numberOfRecommendations).Select(r => r.Beer).ToList();
            }
            return finalRecommendations;
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

      
        public static List<Beer> RecommendBiggestIntersection(List<Beer> pickedPopularBeers,
            Region selectedRegion = null) {
            var tagsFromPickedPopularBeers = Repository.RetrieveTagsFromBeersDictionary(pickedPopularBeers);
            
            var groupedTags = new List<Dictionary<Tag, int>>();
            foreach (var firstPair in tagsFromPickedPopularBeers) {
                var biggestIntersection = new KeyValuePair<Beer, List<Tag>>();
                var intersectionSize = 0;
                intersectionSize = FindIntersection(tagsFromPickedPopularBeers, firstPair, intersectionSize, ref biggestIntersection);
                if (intersectionSize > 3) {
                    var groupedPair = firstPair.Value.Concat(biggestIntersection.Value)
                        .GroupBy(s => s).Select(s => new { Tag = s.Key, Count = s.Count() }).ToDictionary(g => g.Tag, g => g.Count);
                    if (!groupedTags.Contains(groupedPair)) {
                        groupedTags.Add(groupedPair);
                    }
                }
                else {
                    var dictionary = firstPair.Value.ToDictionary(t => t, t => 1);
                    groupedTags.Add(dictionary);
                }
            }


            var allBeers = GetAllBeersForRegion(pickedPopularBeers, selectedRegion).ToList();
            var recommendedBeers = new List<Beer>();
            foreach (var tagGroup in groupedTags)
            {
                var beersContainingSelectedTags = GetBeersContainingSelectedTags(allBeers, tagGroup.Keys.ToList());
                recommendedBeers.Add(
                    GetBeersWithBestCoefficient(1, beersContainingSelectedTags, tagGroup
                    ).First());
            }

            return recommendedBeers.Distinct().ToList();
        }

        private static int FindIntersection(Dictionary<Beer, List<Tag>> tagsFromPickedPopularBeers, KeyValuePair<Beer, List<Tag>> firstPair, int intersectionSize,
            ref KeyValuePair<Beer, List<Tag>> biggestIntersection) {
            foreach (var secondPair in tagsFromPickedPopularBeers) {
                if (firstPair.Key == secondPair.Key) continue;
                var intersection = firstPair.Value.Intersect(secondPair.Value).ToList();
                if (intersection.Count() <= intersectionSize) continue;
                biggestIntersection = secondPair;
                intersectionSize = intersection.Count();
            }
            return intersectionSize;
        }
    }
}