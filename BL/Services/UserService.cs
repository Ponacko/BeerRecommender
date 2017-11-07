using BeerRecommender;
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
        public static bool CreateUser(string userName, int age, string email)
        {
            User user = new User()
            {
                UserName = userName,
                Email = email,
                Age = age
            };

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
    }
}
