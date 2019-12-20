using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TableTennis.DataModels;
using TableTennis.DataClient;
using SPUtility;

namespace TableTennis.Controllers
{
    public class ManagerController : Controller
    {
        public ActionResult Index()
        {
            long? id = (long?)Session["ManagerId"];
            if (id == null)
                return RedirectToAction("SignIn");

            MANAGER dataObj = null;
            Result r = new Result();
            using (ManagerClient client = new ManagerClient())
            {
                r = client.Get(out dataObj, id);
                if (r.Successed)
                {
                    Session["ManagerId"] = dataObj.ID;
                    Session["ManagerName"] = dataObj.FIRST_NAME + " " + dataObj.LAST_NAME;
                }

            }
            return View(dataObj);
        }

        public ActionResult SignUp()
        {
            Uri mUri = Request.Url;
            EmailSender.url = mUri.Scheme + "://" +mUri.Host+ Url.Action("Re", "Manager")+"/";
            MANAGER data = TempData["Manager"] as MANAGER;
            if (data != null)
            {
                if (!string.IsNullOrEmpty(data.FIRST_NAME))
                {
                    ViewBag.firstName = data.FIRST_NAME;
                }

                if (!string.IsNullOrEmpty(data.LAST_NAME))
                {
                    ViewBag.lastNmae = data.LAST_NAME;
                }

                if (!string.IsNullOrEmpty(data.EMAIL))
                {
                    string er = TempData["Error"] as string;
                    ViewBag.exiteEmail = "อีเมลนี้ถูกใช้แล้ว";
                    if (!string.IsNullOrEmpty(er))
                    {
                        if (er== "อีเมลไม่ถูกต้อง")
                        {
                            ViewBag.exiteEmail = er;
                        }
                        
                    }
                    

                    ViewBag.email = data.EMAIL;
                }

                if (!string.IsNullOrEmpty(data.TEAM))
                {
                    ViewBag.team = data.TEAM;
                }

                if (!string.IsNullOrEmpty(data.PHONE))
                {
                    ViewBag.phone = data.PHONE;
                }

                if (data.GENDER != null)
                {
                    ViewBag.gender = data.GENDER;
                }
            }
           

            return View();
        }

        public ActionResult SignIn(string EMAIL , string MANAGER_CODE)
        {
            if (Session["ManagerId"] != null)
                return RedirectToAction("Index", "Tournament");

            if (string.IsNullOrEmpty(EMAIL)||string.IsNullOrEmpty(MANAGER_CODE))
            {                
                return View();
            }

            int id;
            if(!Int32.TryParse(MANAGER_CODE, out id))
            {
                ViewBag.email = EMAIL;
                ViewBag.code = MANAGER_CODE;
                ViewBag.Validate = string.Format("อีเมลหรือระหัสไม่ถูกต้อง");
                return View();
            }

            Result r = new Result();
            MANAGER dataObj = null;
            using(ManagerClient client = new ManagerClient())
            {
                r = client.SignIn(out dataObj, EMAIL, id);                
            }
            if (dataObj == null)
            {
                ViewBag.email = EMAIL;
                ViewBag.code = MANAGER_CODE;
                ViewBag.Validate =string.Format("อีเมลหรือระหัสไม่ถูกต้อง");
                return View();
            }
            else
            {
                Session["ManagerId"] = dataObj.ID;
                Session["ManagerName"] = dataObj.FIRST_NAME + " " + dataObj.LAST_NAME;
            }

            return RedirectToAction("Index","Tournament");
        }

        public ActionResult SignOut()
        {            
            Session.Abandon();
            return RedirectToAction("SignIn");

        }

        public ActionResult Success() 
        {
            long? id =(long?)Session["ManagerId"];
            if (id == null)
                return View();

            Result r = new Result();
            MANAGER dataObj = null;
            using (ManagerClient client = new ManagerClient())
            {
                r = client.Get(out dataObj, id);
                if (r.Successed)
                {
                    Session["ManagerId"] = dataObj.ID;
                    Session["ManagerName"] = dataObj.FIRST_NAME + " " + dataObj.LAST_NAME;
                }
            }
            
            return View(dataObj);
        }

        public ActionResult Recover()
        {
            if (TempData["ErMsg"]!=null)
            {
                ViewBag.Validate = (string)TempData["ErMsg"];
                ViewBag.email = (string)TempData["email"];
            }
           
            return View();
        }

        [HttpPost]
        public ActionResult SendRecover(string EMAIL)
        {
            Uri mUri = Request.Url;
            EmailSender.url = mUri.Scheme + "://" + mUri.Host + Url.Action("Re", "Manager") + "/";
            Result r = new Result();
            MANAGER dataObj = null;
            using (ManagerClient client = new ManagerClient())
            {
                r = client.SendRecover(out dataObj, EMAIL);
            }
            if (!r.Successed)
            {
                TempData["ErMsg"] = r.ErrorMassage;
                TempData["email"] = EMAIL;
                return RedirectToAction("Recover");
            }
            TempData["EMAIL"] = dataObj.EMAIL;
            return RedirectToAction("SuccessRecover");
        }

        public ActionResult SuccessRecover()
        {
            ViewBag.EMAIL = (string)TempData["EMAIL"];
            return View();
        }

        [HttpPost]
        public ActionResult Add(MANAGER dataObj)
        {
            Result r = new Result();
            
            using (ManagerClient client = new ManagerClient()) 
            {
                r = client.Add(ref dataObj);
            }
            if (r.Successed)
            {
                Session["ManagerId"] = dataObj.ID;
                return RedirectToAction("Success");
            }
            else
            {
                TempData["Manager"] = dataObj;
                TempData["Error"] = r.ErrorMassage;
                return RedirectToAction("SignUp");
            }
            
        }

        public ActionResult Re(long? id)
        {
            if (id == null)
                return RedirectToAction("SignUp");

            Session["ManagerId"] = id;
            return RedirectToAction("Index", "Tournament");
        }

        public ActionResult Result()
        {
            return View();
        }

        public ActionResult GridManager(ParamManager param)
        {
            List<MANAGER> dataObj = new List<MANAGER>();
            Result r = new Result();
            using (ManagerClient client = new ManagerClient())
            {
                r = client.GridManager(out dataObj, param);
            }
            return PartialView(dataObj);
        }

        [HttpPost]
        public ActionResult Update(MANAGER dataObj)
        {
            Result r = new Result();
            using (ManagerClient client = new ManagerClient())
            {
                r = client.Update(ref dataObj);
            }
            return  RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete()
        {
            return View();
        }
    }
}