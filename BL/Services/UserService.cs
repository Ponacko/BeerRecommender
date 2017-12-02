using BeerRecommender;
using BeerRecommender.Entities;
using BeerRecommender.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public static class UserService
    {
        public static bool CreateUser(string userName, int age, string email) {
            User user = new User();

            UserRepository userRepo = new UserRepository();
            try
            {
                userRepo.Create(user);
            }
            catch(DbEntityValidationException ex)
            {
                var errors = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);
                var message = string.Join("; ", errors);
                throw new DbEntityValidationException(message, ex.EntityValidationErrors);
            }
            return true;
        }

        public static void AssignRegionToUser(Region region, User user)
        {
            UserRepository userRepo = new UserRepository();
            user.Region = region;
            userRepo.Update(user);
        }

        public static void AddUserPickedBeers(User user, List<Beer> pickedBeers)
        {
            UserRepository ur = new UserRepository();
            ur.RetrieveById(user.Id);
            user.PickedBeers.AddRange(pickedBeers);
            ur.Update(user);    
        }
    }
}
