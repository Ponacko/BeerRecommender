﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BeerRecommender.Entities;

namespace BeerRecommender
{
    public class Brewery : Entity
    {
        public Brewery() {
            Beers = new List<Beer>();
        }
        
        [Required]
        public string Name { get; set; }

        public Region Region { get; set; }
        public string RegionString { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public string WebSiteUrl { get; set; }

        [Range(0, int.MaxValue)]
        public int YearOfFoundation { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<Beer> Beers { get; set; }

        public override string ToString() {
            return $"Brewery {Name} at address {Address}";
        }
    }
}
