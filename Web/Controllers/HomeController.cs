using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BeerRecommender;
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
                var recommended = RecommendationService.ReccomendBeers(beers, 5);
                var id = UserService.CreateUser(beers, recommended, userModel.RegionId);
                return RedirectToAction("Recommend", new {userId = id});
            }
            return View();
        }

        
        public ActionResult Recommend(int userId) {
            
            ViewBag.UserId = userId;
            ViewBag.Region = UserService.GetUserRegion(userId)?.Name;
            var picked = UserService.GetUsersPickedBeers(userId);
            ViewBag.PickedBeers = picked;
            ViewBag.PickedCount = picked.Count;
            ViewBag.RecommendedBeers = UserService.GetUsersRecommendedBeers(userId);
            ViewBag.RandomBeers = RecommendationService.RecommendRandomBeers(5);

            return View();
        }
    }
} ;