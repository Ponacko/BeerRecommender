using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerRecommender.Entities
{
    public class UserSimilarity : Entity
    {
        public User User1 { get; set; }
        public User User2 { get; set; }
        
        public double Similarity { get; set; }
    }
}
