using SPUtility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TableTennis.DataClient;
using System.Data;
using TableTennis.DataModels;

namespace TableTennis.Controllers
{
    public class BaseController : Controller, IDisposable
    {

        // GET: Base
        [HttpPost]
        public JsonResult GetUrlActionEvent()
        {
            return Json(new
            {
                //index
                GetTournamentCode = Url.Action("GetTournamentCode", "admin"),
                GetEvent = Url.Action("GetEvent", "admin"),
                RemoveEvent = Url.Action("RemoveEvent", "admin"),
                AddEvent = Url.Action("AddEvents", "admin"),
                RemoveTour = Url.Action("RemoveTour", "admin"),
                GetFirstTour = Url.Action("GetFirstTour", "admin"),
            });

        }

        [HttpPost]
        public JsonResult GetUrlActionUpload()
        {
            return Json(new
            {
                Logo_Edit = Url.Action("Logo_Edit", "admin"),
                RemoveLogo = Url.Action("RemoveLogo", "admin")
            });

        }

        [HttpPost]
        public JsonResult GetUrlActionUserManager()
        {
            return Json(new
            {
                GetRole = Url.Action("GetRole", "admin"),
                GET_USER_SEARCH = Url.Action("GET_USER_SEARCH", "admin"),
                ADD_USER_ADMIN = Url.Action("ADD_USER_ADMIN", "admin"),
                RemoveUserAdmin = Url.Action("RemoveUserAdmin", "admin"),
                GetGender = Url.Action("GetGender", "admin"),
                UpdateUser = Url.Action("UpdateUser", "admin"),
                managerRe = Url.Action("Re", "Manager"),
            });

        }
        [HttpPost]
        public JsonResult GetUrlActionRemoveTour()
        {
            return Json(new
            {
                RemoveTour = Url.Action("RemoveTour", "admin"),
                GetTournamentCode = Url.Action("GetTournamentCode", "admin"),
            });

        }
        [HttpPost]
        public JsonResult GetUrlActionEditTour()
        {
            return Json(new
            {
                UpdateTournament = Url.Action("UpdateTournament", "admin"),
                GetTournamentCode = Url.Action("GetTournamentCode", "admin"),
                SetData = Url.Action("SetData", "admin"),
            });

        }

        [HttpPost]
        public JsonResult GetUrlActionUploadExcel()
        {
            return Json(new
            {
                Upload = Url.Action("Upload", "admin"),
                CreateRankingID = Url.Action("CreateRankingID", "admin"),
                RemoveRank = Url.Action("RemoveRank", "admin"),
                GET_RANK_DT = Url.Action("GET_RANK_DT", "admin"),
                GET_RANK_DETAIL = Url.Action("GET_RANK_DETAIL", "admin"),
                GET_ERROR_IMPORT_EXCEL = Url.Action("GET_ERROR_IMPORT_EXCEL", "admin"),              

            });

        }

        protected bool IsLogin()
        {
            return !(Session == null || Session["Login"] == null || !(bool)Session["Login"]);
        }

        protected string Hash(string val)
        {
            string salt = "tabletennis";
            byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(val + salt);
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            string hashString = Convert.ToBase64String(hashBytes);
            return hashString;
        }

        public bool Login()
        {
            #region " INITIAL "
            string UserID = Request.Form["UserID"];
            string Password = Request.Form["Password"];

            if (string.IsNullOrEmpty(UserID))
            {
                ViewBag.ErrorMessage = null;
                return false;
            }
            string hashString = Hash(Password);
            TT_LOGIN data = new TT_LOGIN();
            #endregion

            ValidateUser(out data, UserID.Trim().ToUpper(), hashString);
            if (!data.isLogin)
                // RedirectToAction("AdminLogin", "admin");
                return false;
            else
            {
                var aa = data.menu_show.FirstOrDefault().MENU_URL.Split('/');
                ViewBag.ErrorMessage = null;
                ViewBag.Access = data.user_access;
                if (Request.IsLocal)
                {
                    data.menu_show.Select(c => { c.MENU_URL = c.MENU_URL.Replace(aa[2], "Localhost"); return c; }).ToList();
                    Session["MENU"] = data.menu_show;
                }
                else
                {
                    Session["MENU"] = data.menu_show;
                }
                #region " OPERATION "
                if (!IsLogin())
                {
                    Session.Timeout = 30;
                    Session["Login"] = true;
                    Session["DataUser"] = data;
                    //Session["UserID"] = data.dataUser.USER_ID;
                    //Session["UserID"] = data.dataUser.EMAIL;
                    //Session["Password"] = data.dataUser.PASSWORD;
                }
                #endregion

            }
            #region " RETURN "
            //  RedirectToAction("MenuView", "Admin",data.menu_show);
            return true;
            #endregion
        }

        protected DataFile[] getLogo(DataFile[] data, int? tourID)
        {
            DataFile[] dt = new DataFile[0];
            Result pr = new Result();
            var a = (DataFile[])TempData["DataFile"];
            if (tourID != null && a == null)
            {
                using (TournamentClient client = new TournamentClient())
                {
                    pr = client.GET_LOGO(out dt, tourID);
                    if (!pr.Successed)
                    {
                        throw new Exception(pr.ErrorMassage);
                    }
                    else
                    {
                        getFileFromPath(ref dt);
                    }
                }
            }
            else
            {
                dt = a;
            }
            TempData["DataFile"] = dt;
            return dt;
        }

        private void getFileFromPath(ref DataFile[] dt)
        {
            foreach (var item in dt)
            {
                if (System.IO.File.Exists(item.path_file))
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(item.path_file);
                    item.src = Convert.ToBase64String(bytes);
                }
            }
        }

        protected void manageLogo(DataFile[] data)
        {
            var aa = (DataFile[])TempData["DataFile"];
            if (aa != null)
            {
                data.Select(a => { a.header = a.src.Split(',').FirstOrDefault(); a.src = a.src.Split(',').LastOrDefault(); return a; }).ToArray();
                data = data.Concat(aa).ToArray();
            }
            else
            {
                data.Select(a => { a.header = a.src.Split(',').FirstOrDefault(); a.src = a.src.Split(',').LastOrDefault(); return a; }).ToArray();
            }
            TempData["DataFile"] = data;
        }

        protected void manageFile(int? tour_id, TournamentClient client)
        {
            var path = Path.Combine(Server.MapPath("~/uploads"), tour_id.ToString());

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            writeFile(path, client, tour_id);
        }

        protected void pathDelete(int? tour_id)
        {
            var path = Path.Combine(Server.MapPath("~/uploads"), tour_id.ToString());
            deleteDir(path);
        }
        protected void removeLogoFile(DataFile data)
        {
            var a = (DataFile[])TempData["DataFile"];
            if (a != null)
            {
                var v = a.Where(val => val.name != data.name && val.size != data.size).ToArray();
                TempData["DataFile"] = v;
            }
        }

        protected void writeFile(string path, TournamentClient client, int? tour_id)
        {
            Result pr = new Result();
            var data = (DataFile[])TempData["DataFile"];
            if (data != null)
            {
                foreach (var item in data)
                {
                    var bytes = Convert.FromBase64String(item.src);
                    using (var imageFile = new FileStream(path + @"\" + item.name, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }
                    item.uri_path = urlPath(path + @"\" + item.name);
                    item.path_file = path + @"\" + item.name;
                    pr = client.AddLogo(item, tour_id);
                    if (!pr.Successed)
                    {
                        throw new Exception(pr.ErrorMassage);
                    }
                }
            }

        }
        protected string urlPath(string path)
        {
            int length = 0;
            var a = path.Split('\\');
            var b = GetLocalIPAddress();
            length = a.Length;

            string urlPath = b + '/' + a[length - 4] + '/' + a[length - 3] + '/' + a[length - 2] + '/' + a[length - 1];
            return urlPath;
        }
        public string GetLocalIPAddress()
        {
            return Request.Url.Scheme + "://" + Request.Url.Host;

        }
        protected void deleteFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        protected void deleteDir(string path)
        {
            if (Directory.Exists(path))
                System.IO.Directory.Delete(path, true);
        }

        protected void readFile(string folderPath)
        {
            foreach (string file in Directory.EnumerateFiles(folderPath))
            {
                string contents = System.IO.File.ReadAllText(file);
            }
        }

        private void ValidateUser(out TT_LOGIN dataUser, string user, string password)
        {
            Result rs = new Result() { Successed = true };

            using (TournamentClient client = new TournamentClient())
            {
                rs = client.LOGIN_USER(out dataUser, user, password);
                if (!rs.Successed)
                    ViewBag.ErrorMessage = rs.ErrorMassage;
            }
        }

        public ActionResult Logout()
        {
            //clear seesion and somthing
            Session.Abandon();
            return RedirectToAction("AdminLogin", "Admin");
        }
        private string GetUserName(string UserID)
        {
            #region " Grean Checker "
            if (String.IsNullOrEmpty(UserID))
                return "ยังไม่ได้ระบุไอดีผู้ใช้งาน";
            #endregion

            #region " GetFrom CenterSvcRef "
            //using (CenterSvcRef.CenterServiceClient CenterClient = new CenterSvcRef.CenterServiceClient())
            //{
            //    CenterSvcRef.User _User = new CenterSvcRef.User();
            //    CenterSvcRef.ProcessResult _ProcessResult = new CenterSvcRef.ProcessResult();
            //    _User = CenterClient.getUser(out _ProcessResult, UserID);
            //    if (_ProcessResult.Successed)
            //        if (_User != null)
            //        {
            //            return _User.PreName + _User.firstName + " " + _User.lastName;
            //        }
            //        else
            //        {
            //            return "ไม่พบชื่อผู้ใช้งาน";
            //        }
            //    else
            //        return _ProcessResult.ErrorMessage;
            //}
            return "";
            #endregion
        }

        protected List<_TOURNAMENT> SETTourament(List<TOURNAMENT> data)
        {
            List<_TOURNAMENT> temp = new List<_TOURNAMENT>();
            foreach (var item in data)
            {
                _TOURNAMENT _tem = new _TOURNAMENT();
                _tem.ID = item.ID;
                _tem.CODE = item.CODE;
                _tem.EN_TOUR_NAME = item.EN_TOUR_NAME;
                _tem.TH_TOUR_NAME = item.TH_TOUR_NAME;
                _tem.DESCRIPTION = item.DESCRIPTION;
                _tem.TOUR_ADDRESS = item.TOUR_ADDRESS;
                _tem.TOUR_POINT = item.TOUR_POINT;
                _tem.CREATE_ON = item.CREATE_ON;
                _tem.CREATE_BY = item.CREATE_BY;
                _tem.MODIFY_ON = item.MODIFY_ON;
                _tem.MODIFY_BY = item.MODIFY_BY;

                _tem.TENINATE_FLG = item.TENINATE_FLG;

                _tem.TOUR_START_ON = item.TOUR_START_ON;
                _tem.TOUR_START_ON_STR = dateTostring(item.TOUR_START_ON);

                _tem.TOUR_END_ON = item.TOUR_END_ON;
                _tem.TOUR_END_ON_STR = dateTostring(item.TOUR_END_ON);

                _tem.EXPIRE_ON = item.EXPIRE_ON;
                _tem.EXPIRE_ON_STR = dateTostring(item.EXPIRE_ON);
                _tem.RANKING_ID = item.RANKING_ID;
                temp.Add(_tem);
            }
            return temp;
        }

        public string dateTostring(DateTime? data)
        {
            string a = string.Empty;
            CultureInfo ThaiCulture = new CultureInfo("th-TH");
            if (data != null)
            {
                DateTime dt = (DateTime)data;
                a = dt.ToString("dd/MM/yyyy", ThaiCulture);
                var b = a.Split('/');
                b[0] = b[0].PadLeft(2, '0');
                b[1] = b[1].PadLeft(2, '0');
                a = b[0] + "/" + b[1] + "/" + b[2];
            }
            return a;
        }

        protected string authen()
        {
            string userid = string.Empty;
            if (IsLogin())
            {
                var usr = (TT_LOGIN)Session["DataUser"];
                userid = usr.dataUser.ID.ToString();
            }
            return userid;
        }
        protected int? getIdAuthen()
        {
            int? id = null;
            if (IsLogin())
            {
                var usr = (TT_LOGIN)Session["DataUser"];
                id = usr.dataUser.ID;
            }
            return id;
        }
           
    }
}