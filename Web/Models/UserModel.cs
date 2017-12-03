using System.Collections.Generic;
using BeerRecommender.Entities;

namespace Web.Models {
    public class UserModel {
        public int? RegionId { get; set; }
        public List<string> SelectedBeers { get; set; }

        public UserModel() {
               SelectedBeers = new List<string>();
        }
    }
}