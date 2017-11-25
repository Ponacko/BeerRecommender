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
        }

        [Required]
        public string Name { get; set; }
        // Stupnovitost piva
        public double Epm { get; set; }
        public double AlcoholContentPercentage { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public Brewery Brewery { get; set; }
        public string ImageUrl { get; set; }
        public float AverageRating { get; set; } = 0;

        public override string ToString()
        {
            return $"Beer [{Name}] with EPM [{Epm}] with rating [{AverageRating}] of type [{Category}], image available at [{ImageUrl}]";
        }
    }
}
