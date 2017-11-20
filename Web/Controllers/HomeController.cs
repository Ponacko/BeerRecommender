using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL.Services;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() {
            var beers = BeerService.GetAllBeers();
            ViewBag.Beers = beers;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}