using System.Collections.Generic;
using BeerRecommender.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeerRecommender
{
    public class User : Entity
    {
        public User(){}
        
        [StringLength(100)]
        [Required(ErrorMessage = "Invalid Username")]
        public string UserName { get; set; }

        [Index(IsUnique = true)]
        [StringLength(100)]
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [Range(18, 200)]
        public int Age { get; set; }
        
    }
}
