using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TableTennis.Controllers
{
    public class RankingController : Controller
    {
        // GET: Ranking
        public ActionResult Ranking()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveData(List<string> employees)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                //using (MyDatabaseEntities dc = new MyDatabaseEntities())
                //{
                //    foreach (var i in employees)
                //    {
                //        dc.Employees.Add(i);
                //    }
                //    dc.SaveChanges();
                //    status = true;
                //}
            }
            return new JsonResult { Data = new { status = status } };
        }

    }
}