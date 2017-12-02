using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeerRecommender;
using BL.Services;

namespace Web.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            var beers = BeerService.GetPopularBeers().ToList();
            ViewBag.Count = beers.Count();
            ViewBag.Beers = beers;
            var regions = RegionService.GetAllRegions();
            ViewBag.Regions = regions.Select(region => region.Name).ToList(); ;
            return View();
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
} ;