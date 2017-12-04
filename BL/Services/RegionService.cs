using System.Collections.Generic;
using BeerRecommender.Entities;
using BeerRecommender.Repositories;

namespace BL.Services {
    public class RegionService {
        public static List<Region> GetAllRegions()
        {
            var repository = new RegionRepository();
            return repository.RetrieveAll();
        }

        public static Region GetRegion(int id) {
            var repository = new RegionRepository();
            return repository.RetrieveById(id);
        }
    }
}