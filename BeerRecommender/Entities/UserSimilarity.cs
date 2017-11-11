using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerRecommender.Entities
{
    public class UserSimilarity
    {
        public User user1 { get; set; }
        public User user2 { get; set; }
        
        public int Similarity { get; set; }
    }
}
