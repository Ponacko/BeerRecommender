using BeerRecommender.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerRecommender
{
    public class Beer : Entity
    {
        public Beer()
        {
            PickedByUsers = new List<User>();
            RecommendedForUsers = new List<User>();
            Tags = new List<Tag>();
        }

        [Required]
        public string Name { get; set; }
        public List<User> PickedByUsers { get; set; }
        public List<User> RecommendedForUsers { get; set; }
        public List<Tag> Tags { get; set; }
        public bool IsPopular { get; set; }
        // Stupnovitost piva
        public double Epm { get; set; }
        public double AlcoholContentPercentage { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public Brewery Brewery { get; set; }
        public string ImageUrl { get; set; }

        public override string ToString()
        {
            return $"Beer [{Name}] with EPM [{Epm}]  of type [{Category}], image available at [{ImageUrl}]";
        }
    }
}
