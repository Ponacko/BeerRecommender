using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BeerRecommender;
using BeerRecommender.Entities;
using BL.Services;
using Web.Models;

namespace Web.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            var beers = BeerService.GetPopularBeers().Distinct().ToList();
            ViewBag.Count = beers.Count();
            ViewBag.Beers = beers;
            var regions = RegionService.GetAllRegions();
            var dict = regions.ToDictionary(region => region.Id, region => region.Name);
            ViewBag.SelectList = new SelectList(dict, "Key", "Value");
            var model = new UserModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(UserModel userModel) {
            if (ModelState.IsValid) {
                var beers = BeerService.GetBeersByIds(userModel.SelectedBeers);
                
                var id = UserService.CreateUser(beers, userModel.RegionId, RecommendationService.Repository.Context);
                return RedirectToAction("Recommend", new {userId = id});
            }
            return View();
        }

        
        public ActionResult Recommend(int userId) {
            
            ViewBag.UserId = userId;
            var region = UserService.GetUserRegion(userId);
            ViewBag.Region = region?.Name;
            var picked = UserService.GetUsersPickedBeers(userId);
            ViewBag.PickedBeers = picked;
            ViewBag.PickedCount = picked.Count;
            var recommended = RecommendationService.Recommend(picked, 5, region);
            ViewBag.RecommendedBeers = recommended;
            ViewBag.RCount = recommended.Count;
            ViewBag.RandomBeers = RecommendationService.RecommendRandomBeers(5);
            var recommendedSingle = RecommendationService.ReccomendBeersSingle(picked, region);
            ViewBag.RecommendedSingle = recommendedSingle;
            ViewBag.RSCount = recommendedSingle.Count;

            return View();
        }
    }
} ;