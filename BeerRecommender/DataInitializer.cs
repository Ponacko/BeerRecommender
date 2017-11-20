using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HtmlAgilityPack;
using BeerRecommender.Repositories;

namespace BeerRecommender
{
    public class DataInitializer : CreateDatabaseIfNotExists<AppDbContext>
    {
        public const int NUMBER_OF_BREWERY_PAGES = 15;
        public const int NUMBER_OF_BEER_PAGES = 153;

        public DataInitializer(AppDbContext context) {
            Seed(context);
        }

        protected override void Seed(AppDbContext context) {
            if (!context.Breweries.Any() || !context.Beers.Any()) {
                List<HtmlNode> breweries = GetHtmlBreweryNode();
                UpdateBreweryDb(breweries, context);

                List<HtmlNode> beers = GetHtmlBeerNode();
                UpdateBeersDb(beers, context);
            }
            base.Seed(context);
        }

        private static void UpdateBreweryDb(List<HtmlNode> breweries, AppDbContext context)
        {
            foreach (var b in breweries)
            {
                Brewery brewery = GetBreweryFromNode(b);
                AddBreweryToDb(context, brewery);
            }
            context.SaveChanges();
        }

        private static List<HtmlNode> GetHtmlBreweryNode()
        {
            List<HtmlNode> allBreweries = new List<HtmlNode>();
            for (int i = 1; i <= NUMBER_OF_BREWERY_PAGES; i++)
            {
                var web = new HtmlWeb();
                var url = $"http://ceskepivo-ceskezlato.cz/seznam-pivovaru/?pg=" + i;
                var doc = web.Load(url);
                Console.WriteLine($"Parsing page {i}/{NUMBER_OF_BREWERY_PAGES} of breweries...");
                var beers = doc?.DocumentNode?.SelectNodes("//tr[@class='li item']")?.ToList();
                if (beers != null)
                    allBreweries.AddRange(beers);
            }
            return allBreweries;
        }

        private static void AddBreweryToDb(AppDbContext context, Brewery brewery)
        {
            if (!context.Breweries.Any(c => c.Name == brewery.Name))
            {
                context.Breweries.Add(brewery);
            }
        }

        private static Brewery GetBreweryFromNode(HtmlNode b)
        {
            string imageRelativeUrl = b.SelectSingleNode(".//img").GetAttributeValue("src", null).Trim();
            var data = b.SelectNodes(".//td[@class='line']");

            int yearOfFoundation;
            int.TryParse(data.First().SelectSingleNode(".//abbr").InnerText.Trim(), out yearOfFoundation);
            string name = data.First().SelectSingleNode(".//label").InnerText.Trim();
            string city = data.Skip(1).First().SelectSingleNode(".//address/strong").InnerText.Trim();
       
            string address = data.Skip(1).First().SelectSingleNode(".//address").InnerText.Trim();
            int index = address.IndexOf(city);
            string cleanAddress = (index < 0) ? address : address.Remove(index, city.Length);

            var brewery = new Brewery()
            {
                Id = 1,
                Name = name,
                YearOfFoundation = yearOfFoundation,
                City = city,
                Address = address,
                AverageRating = 0,
                ImageUrl = imageRelativeUrl == null ? null : "http://ceskepivo-ceskezlato.cz/" + imageRelativeUrl
            };
            return brewery;
        }

        private static void UpdateBeersDb(List<HtmlNode> beers, AppDbContext context)
        {
            foreach (var b in beers)
            {
                Beer beer = GetBeerFromNode(b, context);
                AddBeerToDb(context, beer);
            }
            context.SaveChanges();
        }

        private static List<HtmlNode> GetHtmlBeerNode()
        {
            List<HtmlNode> allBeers = new List<HtmlNode>();
            for (int i = 1; i <= NUMBER_OF_BEER_PAGES; i++)
            {
                var web = new HtmlWeb();
                var url = $"http://ceskepivo-ceskezlato.cz/seznam-piv/?pg=" + i;
                var doc = web.Load(url);
                Console.WriteLine($"Parsing page {i}/{NUMBER_OF_BEER_PAGES} of beers...");
                var beers = doc.DocumentNode.SelectNodes("//tr[@class='li item']").ToList();
                allBeers.AddRange(beers);
            }
            return allBeers;
        }

        private static void AddBeerToDb(AppDbContext context, Beer beer)
        {
            if (!context.Beers.Any(c => c.Name == beer.Name))
            {
                context.Beers.Add(beer);
            }
        }

        private static Beer GetBeerFromNode(HtmlNode b, AppDbContext breweryContext)
        {
            string imageRelativeUrl = b.SelectSingleNode(".//img").GetAttributeValue("src", null).Trim();
            var data = b.SelectNodes(".//td[@class='line']");

            string name = data.First().SelectSingleNode(".//label").InnerText.Trim();
            string category = data.First().SelectSingleNode(".//var").InnerText.Trim();
            string brewery = data.Skip(1).First().SelectSingleNode(".//address/strong").InnerText;

            BreweryRepository breweryRepository = new BreweryRepository();
            var assignedBrewery = breweryRepository.RetrieveBreweryByName(brewery);

            float rating = 0;
            var beer = new Beer()
            {
                Id = 1,
                Name = name,
                Epm = "0",
                Category = category,
                Brewery = assignedBrewery,
                ImageUrl = imageRelativeUrl == null ? null : "http://ceskepivo-ceskezlato.cz/" + imageRelativeUrl,
                AverageRating = rating
            };
            return beer;
        }
    }
}
