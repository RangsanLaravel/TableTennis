using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TableTennis.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(int? id)
        {
            ViewBag.ManId = id;
            return RedirectToAction("index","Tournament",new { id =id});
        }
    }
}