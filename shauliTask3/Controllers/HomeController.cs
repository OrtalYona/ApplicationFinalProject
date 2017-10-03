using shauliTask3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shauliTask3.Controllers
{        

    public class HomeController : Controller
    {
        private MapsDbContext maps = new MapsDbContext();

        public ActionResult Index()
        {
            //  Maps mofo = null;
            List<Maps> mofo = new List<Maps>();
            foreach (var m in maps.Map)
            {
                mofo.Add(m);
                //  break;
            }

            if (mofo != null)
            {
                

                    return View(mofo.ToList());       

            }
            //else
            //{
            //    ViewBag.Latitude = 51.122;
            //    ViewBag.Longtitude = 0;
            //}

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