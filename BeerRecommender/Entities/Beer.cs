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
        [InverseProperty("PickedBeers")]
        public List<User> PickedByUsers { get; set; }
        [InverseProperty("RecommendedBeers")]
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

        public static bool operator ==(Beer obj1, Beer obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (ReferenceEquals(obj1, null))
            {
                return false;
            }
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }
            return obj1.Id == obj2.Id;
        }

        public static bool operator !=(Beer obj1, Beer obj2)
        {
            return !(obj1 == obj2);
        }

        public override bool Equals(object tag)
        {
            var item = tag as Beer;

            if (item == null)
            {
                return false;
            }

            return Id.Equals(item.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
