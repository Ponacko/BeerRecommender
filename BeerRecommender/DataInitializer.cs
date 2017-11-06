using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace BeerRecommender
{
    public class DataInitializer : CreateDatabaseIfNotExists<AppDbContext>
    {
        public DataInitializer(AppDbContext context) {
            Seed(context);
        }

        protected override void Seed(AppDbContext context) {
            HtmlNodeCollection breweries = GetHtmlBreweryNode();
            UpdateBreweryDb(breweries, context);
            base.Seed(context);
        }

        private static void UpdateBreweryDb(HtmlNodeCollection breweries, AppDbContext context)
        {
            foreach (var b in breweries)
            {
                Brewery brewery = GetBreweryFromNode(b);
                AddBreweryToDb(context, brewery);
            }
            context.SaveChanges();
        }

        private static HtmlNodeCollection GetHtmlBreweryNode()
        {
            var web = new HtmlWeb();
            var url = "https://www.ipivovary.cz/seznam-pivovaru";
            var doc = web.Load(url);
            var breweries = doc.DocumentNode.SelectNodes("//div[@class='rowseznampivo']");
            return breweries;
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
            var attributes = b.SelectNodes(".//div[contains(@class,'seznampivocell')]");
            var ratingStr = attributes[4].InnerText.Trim();
            var rating = string.IsNullOrEmpty(ratingStr) ? 0 : float.Parse(ratingStr, CultureInfo.InvariantCulture);
            var brewery = new Brewery()
            {
                BreweryID = 1,
                Name = attributes[0].InnerText,
                Address = attributes[1].InnerText,
                AverageRating = rating
            };
            return brewery;
        }
    }
}
