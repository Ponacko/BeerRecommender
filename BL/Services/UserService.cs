﻿using BeerRecommender;
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
        public static int CreateUser(List<Beer> pickedBeers, int? regionId, AppDbContext repositoryContext) {

            UserRepository userRepo = new UserRepository(repositoryContext);
            User user = new User();

            if (regionId != null)
            {
                var rr = new RegionRepository();
                var region = rr.RetrieveById((int)regionId);
                userRepo.SetUserRegion(user, region);
            }

            try
            {
                foreach (var pickedBeer in pickedBeers) {
                    userRepo.AddPickedBeer(user, pickedBeer);
                }

                var id = userRepo.Create(user);
                return id;
            }
            catch(DbEntityValidationException ex)
            {
                var errors = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);
                var message = string.Join("; ", errors);
                throw new DbEntityValidationException(message, ex.EntityValidationErrors);
            }
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
            user.PickedBeers.AddRange(pickedBeers);
            ur.Update(user);    
        }

        public static User GetUser(int id) {
            UserRepository ur = new UserRepository();
            return ur.RetrieveById(id);
        }


        public static List<Beer> GetUsersPickedBeers(int userId)
        {
            UserRepository ur = new UserRepository();
            return ur.GetUserPickedBeers(userId);
        }

        public static List<Beer> GetUsersRecommendedBeers(int userId)
        {
            UserRepository ur = new UserRepository();
            return ur.GetUserRecommendedBeers(userId);
        }

        public static Region GetUserRegion(int userId) {
            UserRepository ur = new UserRepository();
            return ur.GetUserRegion(userId);
        }
    }
}
