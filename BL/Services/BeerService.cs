using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeerRecommender;
using BeerRecommender.Repositories;

namespace BL.Services
{
    public class BeerService
    {
        public static List<Beer> GetAllBeers() {
            var repository = new BeerRepository();
            return repository.RetrieveAll();
        }

        public static List<Beer> GetPopularBeers() {
            return GetAllBeers().FindAll(b => b.IsPopular).ToList();
        }
    }
}
