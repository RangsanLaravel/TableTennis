using SPUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TableTennis.DataClient;
using TableTennis.DataModels;

namespace TableTennis.Controllers
{
    public class PlayerController : Controller
    {
        // GET: Player
        public ActionResult Index()
        {
            return RedirectToAction("Result");
        }
        
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(PLAYER dataObj)
        {
            Result r = new Result();
            using (PlayerClient client = new PlayerClient())
            {
                r = client.Add(ref dataObj);
            }
            if (r.Successed)
            {
                TempData["player"] = dataObj;
                return RedirectToAction("Success");

            }
            else
                return RedirectToAction("SignUp");
        }

        public ActionResult Success()
        {
            PLAYER dataObj = (PLAYER)TempData["player"];
            return View(dataObj);
        }

        public ActionResult Result()
        {
            return View();
        }

        public ActionResult GridResult(ParamPlayer param)
        {
            List<PLAYER> dataObj = new List<PLAYER>();
            Result r = new Result();
            using (PlayerClient client = new PlayerClient())
            {
                r = client.GridResult(out dataObj, param);
            }
            return PartialView(dataObj);
        }

        [HttpPost]
        public ActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete()
        {
            return View();
        }
    }
}