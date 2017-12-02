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
            var beers = BeerService.GetPopularBeers().ToList();
            ViewBag.Count = beers.Count();
            ViewBag.Beers = beers;
            var regions = RegionService.GetAllRegions();
            ViewBag.Regions = regions.Select(region => region.Name).ToList();
            var model = new UserModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(UserModel userModel) {
            if (ModelState.IsValid) {
                var beers = BeerService.GetBeersByIds(userModel.SelectedBeers);
                var id = UserService.CreateUser(beers);
                return RedirectToAction("Recommend", new {userId = id});
            }
            return View();
        }

        
        public ActionResult Recommend(int userId) {
            var user = UserService.GetUser(userId);

            ViewBag.UserBeers = user.PickedBeers;

            return View();
        }
    }
} ;