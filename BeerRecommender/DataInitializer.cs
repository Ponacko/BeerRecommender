using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HtmlAgilityPack;
using System.IO;
using System.Text.RegularExpressions;
using BeerRecommender.Entities;
using System.Collections.ObjectModel;
using System.Web.Hosting;

namespace BeerRecommender
{
    public class DataInitializer : CreateDatabaseIfNotExists<AppDbContext>
    {
        #region Constants

        public const int NUMBER_OF_BREWERY_PAGES = 15;
        public const int NUMBER_OF_BEER_PAGES = 153;
        public const string SOURCE_FILENAME = "pivoteky.txt";
        public static readonly IList<String> RegionNames = new ReadOnlyCollection<string>
        (new List<String> {
         "Praha", "Středočeský kraj", "Jihočeský kraj", "Plzeňský kraj",
         "Karlovarský kraj", "Ústecký kraj", "Liberecký kraj", "Královéhradecký kraj", "Pardubický kraj",
         "Kraj Vysočina", "Jihomoravský kraj", "Olomoucký kraj", "Moravskoslezský kraj", "Zlínský kraj" });
        public static readonly IList<String> RegionAbbreviations = new ReadOnlyCollection<string>
        (new List<String> {
         "Praha", "Středo", "Jihočes", "Plzeň",
         "Karlo", "Ústec", "Liber", "Králov", "Pardub",
         "Kraj Vys", "Jihomor", "Olom", "Morav", "Zlín" });
        public static readonly IList<String> PopularBeerNames = new ReadOnlyCollection<string>
        (new List<String> {
         "Pilsner Urquell", "Pardubický Porter", "Primátor India Pale Ale", "Primátor APAč", "Černá Hora Kvasar",
         "Poutník 12% hořký", "Bernard Polotmavá 12", "Velkopopovický Kozel Tmavý nefiltrovaný", "Černá Raketa", "Švihák kávový",
         "Opat ochucené speciální pivo čokoláda", "Primátor Weizenbier", "Otakar 11° nefiltrované", "Primátor Double 24%", "Svijanská Desítka 10%",
         "Bakalář řezané výčepní", "Černá Hora Borůvka", "Bernard Bezlepkový ležák", "Hubertus nealko", "Zlínský Švec 13° jantarový speciál"});

        #endregion

        public static HashSet<Tag> TagSet { get; set; }

        public DataInitializer(AppDbContext context) {
            Seed(context);
        }

        protected override void Seed(AppDbContext context)
        {
            if (!context.Breweries.Any() || !context.Beers.Any())
            {
                TagSet = PrepareBasicTags();
                var parsedObjects = ParseBreweriesAndBeers(SOURCE_FILENAME);
                UpdateBreweryDb(parsedObjects.Item1, context);
                UpdateRegionDb(parsedObjects.Item3, context);
                // Send beers because they contain tags from parsing
                UpdateTagsDb(TagSet, context);
                UpdateBeersDb(parsedObjects.Item2, context);
            }
            else
            {
                TagSet = new HashSet<Tag>(context.Tags.ToList());
            }
            base.Seed(context);
        }

        private static HashSet<Tag> PrepareBasicTags()
        {
            var tagSet = new HashSet<Tag>
            {
                new Tag { Name = "IPA" },
                new Tag { Name = "APA" },
                new Tag { Name = "hořké" },
                new Tag { Name = "10tka" },
                new Tag { Name = "12tka" },
                new Tag { Name = "silné" },
                new Tag { Name = "jantar" }
            };

            return tagSet;
        }

        private static List<Region> PrepareRegionEntities()
        {
            var regions = new List<Region>();
            for(var i = 0; i < RegionNames.Count; i++)
            {
                var region = new Region
                {
                    Id = 1,
                    Name = RegionNames[i],
                    Abbreviation = RegionAbbreviations[i],
                    Breweries = new List<Brewery>()
                };
                regions.Add(region);
            }
            return regions;
        }

        private static Tuple<List<Brewery>, List<Beer>, List<Region>> ParseBreweriesAndBeers(string sourceFileName)
        {
            var breweryList = new List<Brewery>();
            var beerList = new List<Beer>();

            string dirpath = Directory.GetParent(HostingEnvironment.ApplicationPhysicalPath).Parent.FullName;
            string path = Path.Combine(dirpath, @"BeerRecommender\SourceFiles\" + sourceFileName);

            var regionList = PrepareRegionEntities();

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

                    var brewery = GetBreweryFromDocument(doc, ref regionList);
                    if (brewery.Type.Contains("uzavřen"))
                    {
                        Console.WriteLine("  -Brewery has been closed. Skipping...");
                        continue;
                    }
                    breweryList.Add(brewery);
                    beerList.AddRange(GetBeersFromDocument(doc, brewery));
                }
            }

            return new Tuple<List<Brewery>, List<Beer>, List<Region>>(breweryList, beerList, regionList);
        }

        #region BreweryParsing

        private static string getBreweryAttributeFromDocument(HtmlDocument doc, string attributeName, int index = -1)
        {
            if (index != -1)
            {
                return doc?.DocumentNode?.SelectNodes($"//table[@class='info']//td[@title='{attributeName}']")?.ToList()[index]?.InnerText;
            }
            return doc?.DocumentNode?.SelectNodes($"//table[@class='info']//td[@title='{attributeName}']")?.ToList()?.First()?.InnerText;
        }

        private static Brewery GetBreweryFromDocument(HtmlDocument doc, ref List<Region> regions)
        {
            Regex cityRegex = new Regex("(.)*(nbsp;)");

            string name = getBreweryAttributeFromDocument(doc, "Jméno pivovaru");
            string type = getBreweryAttributeFromDocument(doc, "Typ pivovaru");
            string address = getBreweryAttributeFromDocument(doc, "Adresa pivovaru");
            string city = cityRegex.Replace(getBreweryAttributeFromDocument(doc, "Adresa pivovaru", 1), "");
            string regionName = getBreweryAttributeFromDocument(doc, "Adresa pivovaru", 2);
            string webSite = getBreweryAttributeFromDocument(doc, "Domovské stránky");

            string imageRelativeUrl = doc?.DocumentNode?.SelectNodes("//img[@class='logotyp']")?.ToList()?.First()?.GetAttributeValue("src", null);
            string foundationYearString = doc?.DocumentNode?.SelectNodes("//span[@title='Založení pivovaru']")?.ToList()?.First()?.InnerText;
            int yearOfFoundation = 0;

            if (foundationYearString != null)
            {
                int.TryParse(Regex.Match(foundationYearString, @"\d+").Value, out yearOfFoundation);
            }

            var result = regions.Where(r => regionName.StartsWith(r.Abbreviation));
            var breweryRegion = result.Any() ? result.First() : null; 
            var brewery = new Brewery()
            {
                Name = name,
                Type = type,
                YearOfFoundation = yearOfFoundation,
                City = city,
                Address = address,
                Region = breweryRegion,
                WebSiteUrl = webSite,
                ImageUrl = imageRelativeUrl == null ? null : "http://ceskepivo-ceskezlato.cz/" + imageRelativeUrl
            };

            if ( breweryRegion != null ) breweryRegion.Breweries.Add(brewery);

            return brewery;
        }

        #endregion

        #region BeerParsing

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

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            var cleanedTags = regex.Replace(tags, " ");
            var stringTagList = cleanedTags.Split(' ').Where(t => t != "");

            var tagsList = new List<Tag>();
            foreach (var stringTag in stringTagList)
            {
                string tagName = stringTag.Trim();
                if (stringTag.Length < 4) continue;
                if (stringTag.Last() == 'ý')
                    tagName = stringTag.Remove(stringTag.Length - 1, 1) + "é";

                var tagCondition = TagSet.Where(t => t.Name == tagName);
                if (tagCondition.Any())
                {
                    tagsList.Add(tagCondition.First());
                }
                else
                {
                    var tag = new Tag
                    {
                        Name = tagName
                    };
                    TagSet.Add(tag);
                    tagsList.Add(tag);
                }
            }
            if (name.Contains("IPA") || name.Contains("India Pale Ale"))
            {
                tagsList.Add(TagSet.Where(t => t.Name == "IPA").First());
            }
            if (name.Contains("APA"))
            {
                tagsList.Add(TagSet.Where(t => t.Name == "APA").First());
            }
            if (name.Contains("hořk"))
            {
                tagsList.Add(TagSet.Where(t => t.Name == "hořké").First());
            }
            if (name.Contains("10") || name.Contains("Desít"))
            {
                tagsList.Add(TagSet.Where(t => t.Name == "10tka").First());
            }
            if (name.Contains("12") || name.Contains("Dvan"))
            {
                tagsList.Add(TagSet.Where(t => t.Name == "12tka").First());
            }
            if (description.Contains("nejsiln"))
            {
                tagsList.Add(TagSet.Where(t => t.Name == "silné").First());
            }
            if (name.Contains("jantar"))
            {
                tagsList.Add(TagSet.Where(t => t.Name == "jantar").First());
            }

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
                Name = name,
                Description = description,
                Epm = epm,
                AlcoholContentPercentage = alcoholContent,
                Category = tags,
                Brewery = brewery,
                ImageUrl = imageRelativeUrl == null ? null : "http://ceskepivo-ceskezlato.cz/" + imageRelativeUrl,
                Tags = tagsList,
                IsPopular = PopularBeerNames.Contains(name) ? true : false
            };

            return beer;
        }

        #endregion

        #region UpdateDB
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

        private static void UpdateTagsDb(HashSet<Tag> beers, AppDbContext context)
        {
            foreach (var t in TagSet)
            {
                AddTagToDb(context, t);
            }
            context.SaveChanges();
        }

        private static void UpdateRegionDb(List<Region> regions, AppDbContext context)
        {
            foreach (var r in regions)
            {
                AddRegionToDb(context, r);
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

        private static void AddTagToDb(AppDbContext context, Tag tag)
        {
            if (!context.Tags.Any(c => c.Name == tag.Name))
            {
                context.Tags.Add(tag);
            }
        }

        private static void AddBeerToDb(AppDbContext context, Beer beer)
        {
            if (!context.Beers.Any(c => c.Name == beer.Name))
            {
                context.Beers.Add(beer);
            }
        }

        private static void AddRegionToDb(AppDbContext context, Region region)
        {
            if (!context.Regions.Any(c => c.Name == region.Name))
            {
                context.Regions.Add(region);
            }
        }
        #endregion
    }
}
