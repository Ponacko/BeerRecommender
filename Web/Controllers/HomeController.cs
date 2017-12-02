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
            var beers = BeerService.GetAllBeers().Take(30);
            var enumerable = beers as Beer[] ?? beers.ToArray();
            ViewBag.Beers = enumerable.ToList();
            ViewBag.Count = enumerable.Count();
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