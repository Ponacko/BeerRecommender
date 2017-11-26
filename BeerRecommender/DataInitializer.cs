using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HtmlAgilityPack;
using System.IO;
using System.Text.RegularExpressions;

namespace BeerRecommender
{
    public class DataInitializer : CreateDatabaseIfNotExists<AppDbContext>
    {
        public const int NUMBER_OF_BREWERY_PAGES = 15;
        public const int NUMBER_OF_BEER_PAGES = 153;
        public const string SOURCE_FILENAME = "pivoteky.txt";

        public DataInitializer(AppDbContext context) {
            Seed(context);
        }

        protected override void Seed(AppDbContext context)
        {
            if (!context.Breweries.Any() || !context.Beers.Any())
            {
                var parsedObjects = ParseBreweriesAndBeers(SOURCE_FILENAME);

                UpdateBreweryDb(parsedObjects.Item1, context);
                UpdateBeersDb(parsedObjects.Item2, context);
            }
            base.Seed(context);
        }

        private static Tuple<List<Brewery>, List<Beer>> ParseBreweriesAndBeers(string sourceFileName)
        {
            var breweryList = new List<Brewery>();
            var beerList = new List<Beer>();

            string dirpath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string path = Path.Combine(dirpath, @"SourceFiles\" + sourceFileName);

            int lines = File.ReadAllLines(path).Length;
            int counter = 0;
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                    Console.WriteLine($"Parsing Brewery {++counter}/{lines}");
                    string url = sr.ReadLine();
                    var web = new HtmlWeb();
                    var doc = web.Load(url);

                    var brewery = GetBreweryFromDocument(doc);
                    if (brewery.Type.Contains("uzavřen"))
                    {
                        Console.WriteLine("  -Brewery has been closed. Skipping...");
                        continue;
                    }
                    breweryList.Add(brewery);
                    beerList.AddRange(GetBeersFromDocument(doc, brewery));
                }
            }

            return new Tuple<List<Brewery>, List<Beer>>(breweryList, beerList);
        }

        private static string getBreweryAttributeFromDocument(HtmlDocument doc, string attributeName, int index = -1)
        {
            if (index != -1)
            {
                return doc?.DocumentNode?.SelectNodes($"//table[@class='info']//td[@title='{attributeName}']")?.ToList()[index]?.InnerText;
            }
            return doc?.DocumentNode?.SelectNodes($"//table[@class='info']//td[@title='{attributeName}']")?.ToList()?.First()?.InnerText;
        }

        private static Brewery GetBreweryFromDocument(HtmlDocument doc)
        {
            Regex cityRegex = new Regex("(.)*(nbsp;)");

            string name = getBreweryAttributeFromDocument(doc, "Jméno pivovaru");
            string type = getBreweryAttributeFromDocument(doc, "Typ pivovaru");
            string address = getBreweryAttributeFromDocument(doc, "Adresa pivovaru");
            string city = cityRegex.Replace(getBreweryAttributeFromDocument(doc, "Adresa pivovaru", 1), "");
            string region = getBreweryAttributeFromDocument(doc, "Adresa pivovaru", 2);
            string webSite = getBreweryAttributeFromDocument(doc, "Domovské stránky");

            string imageRelativeUrl = doc?.DocumentNode?.SelectNodes("//img[@class='logotyp']")?.ToList()?.First()?.GetAttributeValue("src", null);
            string foundationYearString = doc?.DocumentNode?.SelectNodes("//span[@title='Založení pivovaru']")?.ToList()?.First()?.InnerText;
            int yearOfFoundation = 0;

            if (foundationYearString != null)
            {
                int.TryParse(Regex.Match(foundationYearString, @"\d+").Value, out yearOfFoundation);
            }

            var brewery = new Brewery()
            {
                Id = 1,
                Name = name,
                Type = type,
                YearOfFoundation = yearOfFoundation,
                City = city,
                Address = address,
                RegionString = region,
                WebSiteUrl = webSite,
                ImageUrl = imageRelativeUrl == null ? null : "http://ceskepivo-ceskezlato.cz/" + imageRelativeUrl
            };
            return brewery;
        }

        private static List<Beer> GetBeersFromDocument(HtmlDocument doc, Brewery brewery)
        {
            var beersForBrewery = new List<Beer>();
            var tableElements = doc?.DocumentNode?.SelectNodes("//div[@class='beer-box']")?.ToList();
            if (tableElements != null)
            {
                for (var i = 0; i < tableElements.Count; i++)
                {
                    Console.WriteLine($"    Parsing Beer {i + 1}/{tableElements.Count}");
                    var beer = GetBeerFromNode(tableElements[i], brewery);
                    if (beer.Description.Contains("Vaření piva ukončeno."))
                    {
                        Console.WriteLine($"    --Beer is now longer being brewed. Skipping...");
                        continue;
                    }
                    beersForBrewery.Add(beer);
                }
            }
            return beersForBrewery;
        }

        private static Beer GetBeerFromNode(HtmlNode node, Brewery brewery)
        {
            Regex cityRegex = new Regex("(.)*(nbsp;)");
            string name = node.SelectSingleNode(".//div[@class='beer-title']//h4")?.InnerText;
            string description = node.SelectSingleNode(".//div[@class='beer-text']")?.InnerText;
            string tags = node.SelectSingleNode(".//div[@class='beer-spec']//span")?.InnerText;
            string epmAndAlcohol = node.SelectNodes(".//div[@class='beer-spec']/strong")?.ToList()?.Last().InnerText;

            double epm = 0.0;
            double alcoholContent = 0.0;
            if (epmAndAlcohol != null)
            {
                var splitString = epmAndAlcohol.Split('/');
                if (splitString.Count() >= 1) double.TryParse(Regex.Match(epmAndAlcohol.Split('/').First(), @"([0-9]*[.])?[0-9]+").Value, out epm);
                if (splitString.Count() == 2) double.TryParse(Regex.Match(epmAndAlcohol.Split('/').Last(), @"([0-9]*[.])?[0-9]+").Value, out alcoholContent);
            }

            string imageRelativeUrl = node.SelectSingleNode(".//div[@class='beer-view']//img")?.GetAttributeValue("src", null);

            var beer = new Beer()
            {
                Id = 1,
                Name = name,
                Description = description,
                Epm = epm,
                AlcoholContentPercentage = alcoholContent,
                Category = tags,
                Brewery = brewery,
                ImageUrl = imageRelativeUrl == null ? null : "http://ceskepivo-ceskezlato.cz/" + imageRelativeUrl,
            };

            return beer;
        }

        private static void UpdateBreweryDb(List<Brewery> breweries, AppDbContext context)
        {
            foreach (var b in breweries)
            {
                AddBreweryToDb(context, b);
            }
            context.SaveChanges();
        }

        private static void UpdateBeersDb(List<Beer> beers, AppDbContext context)
        {
            foreach (var b in beers)
            {
                AddBeerToDb(context, b);
            }
            context.SaveChanges();
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
                ImageUrl = imageRelativeUrl == null ? null : "http://ceskepivo-ceskezlato.cz/" + imageRelativeUrl
            };
            return brewery;
        }

        private static void AddBeerToDb(AppDbContext context, Beer beer)
        {
            if (!context.Beers.Any(c => c.Name == beer.Name))
            {
                context.Beers.Add(beer);
            }
        }
    }
}
