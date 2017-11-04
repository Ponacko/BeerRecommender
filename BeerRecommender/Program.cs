using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace BeerRecommender
{
    class Program
    {
        static void Main(string[] args)
        {
            var web = new HtmlWeb();
            var url = "https://www.ipivovary.cz/seznam-pivovaru";
            var doc = web.Load(url);
            var pivovary = doc.DocumentNode.SelectNodes("//div[@class='rowseznampivo']");
            foreach (var pivovar in pivovary) {
                var attributes = pivovar.SelectNodes(".//div[contains(@class,'seznampivocell')]");
                Console.WriteLine($"Pivovar {attributes[0].InnerText} na adrese {attributes[1].InnerText} ma hodnotenie {attributes[4].InnerText}.");
            }
            Console.ReadLine();
        }
    }
}
