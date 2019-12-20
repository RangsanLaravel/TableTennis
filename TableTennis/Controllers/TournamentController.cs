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
    public class TournamentController : Controller
    {
        // GET: Tournament
        public ActionResult Index()
        {
            long? managerId = (long?)Session["ManagerId"];
            MANAGER Manager = null;
            using (ManagerClient client = new ManagerClient())
            {
                Result re = new Result();
                re = client.Get(out Manager, managerId);
                if (re.Successed)
                {
                    if (Manager != null)
                    {
                        ViewBag.id = Manager.ID;
                        Session["ManagerId"] = Manager.ID;
                        Session["ManagerName"] = Manager.FIRST_NAME + " " + Manager.LAST_NAME;
                    }
                    
                }
            }

            Result r = new Result();
            List<TOURNAMENT> dataObj = new List<TOURNAMENT>();
            using (TournamentClient client = new TournamentClient())
            {
                r = client.Gets(out dataObj);
            }
            return View(dataObj);
        }

        public ActionResult Get()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Register(int? id,int? tourId,int? catId)
        {            
            REGISTER dataObj = new REGISTER();
            dataObj.catMapTour = new CAT_MAP_TOUR();
            
            using (TournamentClient client = new TournamentClient())
            {
                Result r = new Result();
                dataObj._catMapTour = new List<CAT_MAP_TOUR>();
                List<CAT_MAP_TOUR> mapTour = new List<CAT_MAP_TOUR>();
                r = client.GetCatMapTours(out mapTour, tourId);
                if (r.Successed)
                {
                    dataObj._catMapTour = mapTour;
                    dataObj.catMapTour = (from x in mapTour where x.CAT_ID == catId select x).FirstOrDefault();
                }
            }

            using (TournamentClient client = new TournamentClient())
            {
                Result r = new Result();
                dataObj.tournament = new TOURNAMENT();
                TOURNAMENT tour = new TOURNAMENT();
                r = client.Get(out tour, tourId);
                if (r.Successed)
                    dataObj.tournament = tour;
            }
            using (ManagerClient client = new ManagerClient())
            {
                Result r = new Result();
                dataObj.manager = new MANAGER();
                MANAGER manager = new MANAGER();
                r = client.Get(out manager, id);
                if (r.Successed)
                    dataObj.manager = manager;
            }

            using (TournamentClient client = new TournamentClient())
            {
                Result r = new Result();
                dataObj._event = new List<EVENT>();
                List<EVENT> _event = new List<EVENT>();
                r = client.GetEvent(out _event ,dataObj.tournament.ID,catId);
                if (r.Successed)
                    dataObj._event = _event;
            }

            return View(dataObj);
        }

        [HttpPost]
        public JsonResult Player(int? id,int? managerId,int? tourId,int catId,int? evenId)
        {
            if (id == 0 || id == null)
                return Json(new
                {
                    result = new Result
                    {
                        Successed = false,
                        ErrorMassage ="เฉพาะรหัสนักกีฬาเท่านั้น"
                    }
                },JsonRequestBehavior.AllowGet);

            Result r = new Result();
            using (TournamentClient client = new TournamentClient())
            {
                r = client.HasRegisted(id, managerId, tourId, catId, evenId);
            }

            if (!r.Successed)
            {
                return Json(new
                {                    
                    result = r
                }, JsonRequestBehavior.AllowGet);
            }

            PLAYER dataObj = new PLAYER();
            using(PlayerClient client = new PlayerClient())
            {
                r = client.Get(out dataObj, id);
            }

            return Json(new {
                data = dataObj,
                result = r
            },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddTourRegister(TOUR_REGISTER dataObj)
        {
            Result r = new Result();
            using (TournamentClient client = new TournamentClient())
            {
                r = client.AddTourRegister(ref dataObj);
            }

            return Json(new
            {
                data = dataObj,
                result = r
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddTourRegisters(TOUR_REGISTER[] dataObj) {
            Result r = new Result();
            string teamRef = "";
            if (dataObj.Any())
            {
                DateTime t = DateTime.Now;
                teamRef = dataObj.FirstOrDefault().MANAGER_ID.ToString() + t.ToString("yyyyMMddHHmmss");
            }

            using (TournamentClient client = new TournamentClient())
            {
                foreach(TOUR_REGISTER item in dataObj)
                {
                    item.TEAM_REF = teamRef;
                    TOUR_REGISTER data = item;
                    r = client.AddTourRegister(ref data);
                }
            }

            return Json(new
            {
                data = dataObj,
                result = r
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetTourRegister(int? managerId,int? tourId)
        {
            Result r = new Result();
            List<TOUR_REGISTER_VIEW> dataObj = new List<TOUR_REGISTER_VIEW>();
            using (TournamentClient client = new TournamentClient())
            {
                r = client.GetTourRegister(out dataObj, managerId,tourId,1);
            }

            return PartialView(dataObj);
        }

        [HttpPost]
        public ActionResult Add()
        {
            return View();
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

        [HttpPost]
        public JsonResult RemovePlayer(int? regId)
        {
            Result r = new Result();
            using (TournamentClient client = new TournamentClient())
            {
                r = client.RemovePlayer(regId);
            }

            return Json(new {result = r  },JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RemoveTeam(string team_ref)
        {
            Result r = new Result();
            using (TournamentClient client = new TournamentClient())
            {
                r = client.RemoveTeam(team_ref);
            }

            return Json(new { result = r }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Result(int? tourId,int? catId)
        {
            //tourId = 1;
            REGISTER dataObj = new REGISTER();
            dataObj.catMapTour = new CAT_MAP_TOUR();

            using (TournamentClient client = new TournamentClient())
            {
                Result r = new Result();
                dataObj._catMapTour = new List<CAT_MAP_TOUR>();
                List<CAT_MAP_TOUR> mapTour = new List<CAT_MAP_TOUR>();
                r = client.GetCatMapTours(out mapTour, tourId);
                if (r.Successed)
                {
                    dataObj._catMapTour = mapTour;
                    dataObj.catMapTour = (from x in mapTour where x.CAT_ID == catId select x).FirstOrDefault();
                }
            }

            using (TournamentClient client = new TournamentClient())
            {
                Result r = new Result();
                dataObj.tournament = new TOURNAMENT();
                TOURNAMENT tour = new TOURNAMENT();
                r = client.Get(out tour, tourId);
                if (r.Successed)
                    dataObj.tournament = tour;
            }
            
            using (TournamentClient client = new TournamentClient())
            {
                Result r = new Result();
                dataObj._event = new List<EVENT>();
                List<EVENT> _event = new List<EVENT>();
                r = client.GetEvent(out _event, dataObj.tournament.ID, catId);
                if (r.Successed)
                    dataObj._event = _event;
            }

            using (TournamentClient client = new TournamentClient())
            {
                Result r = new Result();
                dataObj.slTeam = new List<SL_TEAM>();
                List<SL_TEAM> slTeam = new List<SL_TEAM>();
                r = client.GetSlTeam(out slTeam, dataObj.tournament.ID, catId);
                if (r.Successed)
                    dataObj.slTeam = slTeam;
            }

             return View(dataObj);
        }

        public ActionResult GridResult(int? tourId,int? catId, ParamTourResult param)
        {
            Result r = new Result();
            List<TOUR_REGISTER_VIEW> dataObj = new List<TOUR_REGISTER_VIEW>();
            using (TournamentClient client = new TournamentClient())
            {
                r = client.GetTourResult(out dataObj, param, tourId, catId);
            }
            return PartialView(dataObj);
        }

        public ActionResult GridTeamResult(int? tourId, int? catId, ParamTourResult param)
        {
            Result r = new Result();
            List<TOUR_REGISTER_VIEW> dataObj = new List<TOUR_REGISTER_VIEW>();
            using (TournamentClient client = new TournamentClient())
            {
                r = client.GetTourResult(out dataObj, param, tourId, catId);
            }
            return PartialView(dataObj);
        }

        public ActionResult GetTeamRegister(int? managerId, int? catId, int? tourId)
        {
            Result r = new Result();
            List<TOUR_REGISTER_VIEW> dataObj = new List<TOUR_REGISTER_VIEW>();
            using (TournamentClient client = new TournamentClient())
            {
                r = client.GetTourRegister(out dataObj, managerId, tourId, catId);
            }

            return PartialView(dataObj);
        }

        public JsonResult UrlTour()
        {
            return Json(new {
                AddTourRegister = Url.Action("AddTourRegister", "Tournament"),
                GetTourRegister = Url.Action("GetTourRegister", "Tournament"),
                Player = Url.Action("Player", "Tournament"),
                RemovePlayer = Url.Action("RemovePlayer", "Tournament"),
                AddTourRegisters = Url.Action("AddTourRegisters", "Tournament"),
                GetTeamRegister = Url.Action("GetTeamRegister", "Tournament"),
                RemoveTeam = Url.Action("RemoveTeam", "Tournament")
            }, JsonRequestBehavior.AllowGet);
        } 
    }
}