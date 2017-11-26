using System.Collections.Generic;
using BeerRecommender.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeerRecommender
{
    public class User : Entity
    {
        public User() {
            PickedBeers = new List<Beer>();
            RecommendedBeers = new List<Beer>();
        }

        public Region Region { get; set; }
        public List<Beer> PickedBeers { get; set; }
        public List<Beer> RecommendedBeers { get; set; }
    }
}
