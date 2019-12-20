using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TableTennis.DataModels;
using SPUtility;
using TableTennis.DataClient;
using System.IO;
using System.Data;
using ExcelDataReader;

namespace TableTennis.Controllers
{
    public class AdminController : BaseController
    {
        // GET: Admin       

        #region " View "

        #region " Tournament "
        public ActionResult Index()
        {
            Result r = new Result();
            List<RANKING> dataObject = new List<RANKING>();
            if (!IsLogin())
            {
                return RedirectToAction("AdminLogin");
            }
            try
            {
                using (TournamentClient client = new TournamentClient())
                {
                    r = client.GET_RANKING(out dataObject);
                    if (!r.Successed)
                        throw new Exception(r.ErrorMassage);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMesasge = ex.Message;
            }
            return View(dataObject);

        }
        public ActionResult RemoveTournament()
        {
            if (!IsLogin())
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        public ActionResult EditTournament()
        {
            if (!IsLogin())
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        #endregion

        #region " Menu " 
        public ActionResult MenuView()
        {
            if (!IsLogin())
            {
                return RedirectToAction("AdminLogin");
            }
            var a = TempData["DataFile"];
            return View();
        }
        #endregion

        #region " Add Event"
        public ActionResult AddEvent()
        {
            if (!IsLogin())
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        #endregion

        #region " Login "      
        public ActionResult AdminLogin()
        {

            if (Login())
                return RedirectToAction("MenuView");
            else
            {
                return View();
            }

        }
        #endregion

        #region " Add User "
        public ActionResult UserManager()
        {
            if (!IsLogin())
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        #endregion

        #region " File "
        public ActionResult UploadLogo()
        {
            if (!IsLogin())
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        public ActionResult ExcelApi()
        {
            return View();
        }
        public ActionResult ExcelUpLoad()
        {
            Result r = new Result();
            List<RANKING> dataObject = new List<RANKING>();
            if (!IsLogin())
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                #region " OPERATION "
                try
                {
                    using (TournamentClient client = new TournamentClient())
                    {
                        r = client.GET_RANKING(out dataObject);
                        if (!r.Successed)
                            throw new Exception(r.ErrorMassage);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMesasge = ex.Message;
                }
                #endregion

            }
            return View(dataObject);
        }


        #endregion

        #region " Error "
        public ActionResult Error()
        {
            return View();
        }
        #endregion

        #region " UserEdit "
        public ActionResult UserManagerEdit()
        {
            if (!IsLogin())
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }
        #endregion

        #region " Ranking "
        public ActionResult ManageRanking()
        {
            return View();
        }
        #endregion

        #endregion

        #region " ADD DATA "
        public ActionResult AddTournament(_TOURNAMENT dataObject)
        {
            if (!IsLogin())
            {
                return RedirectToAction("AdminLogin");
            }
            Result rs = new Result() { Successed = true };
            JsonResponse js = new JsonResponse() { data = null, session = true, Successed = true };
            int? tour_id = null;
            try
            {
                using (TournamentClient client = new TournamentClient())
                {
                    dataObject.CREATE_BY = authen();
                    dataObject.MODIFY_BY = authen();
                    rs = client.Add(out tour_id, dataObject);
                    if (!rs.Successed)
                        throw new Exception(rs.ErrorMassage);
                    else
                    {
                        if (tour_id == null)
                            throw new Exception("tour_id IS null ");
                        manageFile(tour_id, client);
                    }
                }
                TempData["fTourID"] = tour_id;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Error", "Admin");
            }

            return RedirectToAction("AddEvent", "Admin");
        }

        public JsonResult AddEvents(int? TOUR_ID, EVENT[] dataObjects, int? category, string num)
        {
            JsonResponse js = new JsonResponse();

            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    #region " OPERATION "
                    if (TOUR_ID == null)
                        throw new Exception("กรุณาเลือก ทัวร์นาเม้นก่อน");
                    int n;
                    if (!int.TryParse(num, out n))
                    {
                        throw new Exception("กรุณาใส่จำนวนนักกีฬา");
                    }
                    if (category == null)
                        category = 1;

                    Result rs = new Result() { Successed = true };
                    using (TournamentClient client = new TournamentClient())
                    {
                        string user = authen();
                        dataObjects.Select(a => { a.CREATE_BY = user; a.MODIFY_BY = user; return a; }).ToArray();
                        rs = client.AddEvent(TOUR_ID, dataObjects, category, n);
                        if (rs.Successed)
                            js = new JsonResponse() { Successed = true, session = true, };
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                js = new JsonResponse() { Successed = false, session = true, ErrorMassage = ex.Message };
            }

            return Json(js);
        }

        public JsonResult ADD_USER_ADMIN(TT_USER condition)
        {
            Result r = new Result();
            JsonResponse js;
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    #region " OPERATION "
                    if (condition.ROLE_ID == null)
                        throw new Exception("กรุณาเลือก สิทธิ์ผู้ดูแลลระบบ");
                    if (condition.ROLE_ID != 1)
                    {
                        throw new Exception("เพิ่มได้เฉพาะสิทธิ์ ผู้ดูแลระบบเท่านั้น");
                    }
                    if (string.IsNullOrEmpty(condition.EMAIL))
                    {
                        throw new Exception("กรุณาใส่ Email ");
                    }
                    if (string.IsNullOrEmpty(condition.USER_NAME))
                    {
                        throw new Exception("กรุณาใส่ ชื่อ ");
                    }
                    if (string.IsNullOrEmpty(condition.USER_SURNAME))
                    {
                        throw new Exception("กรุณาใส่ นามสกุล ");
                    }
                    if (string.IsNullOrEmpty(condition.PASSWORD))
                    {
                        throw new Exception("กรุณาใส่ Password ");
                    }
                    condition.PASSWORD = Hash(condition.PASSWORD);

                    using (TournamentClient client = new TournamentClient())
                    {
                        r = client.ADD_USER_ADMIN(condition);
                        if (r.Successed)
                            js = new JsonResponse() { Successed = true, session = true, data = null };
                        else
                            throw new Exception(r.ErrorMassage);
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                js = new JsonResponse() { Successed = false, session = true, ErrorMassage = ex.Message };
            }
            return Json(js);
        }
        #endregion

        #region " GET DATA "

        public JsonResult GET_ERROR_IMPORT_EXCEL()
        {
            JsonResponse js = new JsonResponse() { Successed = false };
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    #region " OPERATION "
                    var errorMessage = (string)TempData["ExcelError"];
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        if (errorMessage == "true")
                        {
                            js = new JsonResponse() { Successed = true, session = true ,data = errorMessage};
                        }
                        else
                        {
                            throw new Exception(errorMessage);
                        }
                            
                    }  else
                    { js = new JsonResponse() { Successed = true, session = true }; }                  
                    #endregion
                }
            }
            catch (Exception ex)
            {

                js = new JsonResponse() { Successed = false, session = true, ErrorMassage = ex.Message };
            }

            return Json(js);
        }
        public JsonResult GET_RANK_DT(RANKING data)
        {
            Result r = new Result();
            List<RANKING_DETAIL> rank = new List<RANKING_DETAIL>();
            JsonResponse js = new JsonResponse() { Successed = false };

            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    #region " OPERATION "
                    using (TournamentClient client = new TournamentClient())
                    {
                        r = client.GET_RANK_DT(out rank, data.ID);
                        if (!r.Successed)
                            throw new Exception(r.ErrorMassage);
                    }
                    js = new JsonResponse() { Successed = true, session = true, data = rank };
                    #endregion
                }
            }
            catch (Exception ex)
            {

                js = new JsonResponse() { Successed = false, session = true, ErrorMassage = ex.Message };
            }

            return Json(js);
        }
        public JsonResult GET_RANK_DETAIL(RANKING_DETAIL data)
        {
            Result r = new Result();
            List<RANKING_DETAIL> rank = new List<RANKING_DETAIL>();
            JsonResponse js = new JsonResponse() { Successed = false };
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    #region " OPERATION "
                    using (TournamentClient client = new TournamentClient())
                    {
                        r = client.GET_RANK_DETAIL(out rank, data.RANKING_ID, data.RANK_CODE);
                        if (!r.Successed)
                            throw new Exception(r.ErrorMassage);
                    }
                    js = new JsonResponse() { Successed = true, session = true, data = rank };
                    #endregion
                }
            }
            catch (Exception ex)
            {

                js = new JsonResponse() { Successed = false, session = true, ErrorMassage = ex.Message };
            }

            return Json(js);
        }
        public JsonResult GetTournamentCode()
        {

            Result r = new Result();
            JsonResponse js = new JsonResponse() { Successed = false };
            List<TOURNAMENT> dataObj = new List<TOURNAMENT>();
            List<CATEGORY> cat = new List<CATEGORY>();
            List<_TOURNAMENT> _dataObj = new List<_TOURNAMENT>();
            List<RANKING> rnk = new List<RANKING>();
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    #region " OPERATION "
                    using (TournamentClient client = new TournamentClient())
                    {
                        r = client.GET_TOUR_CODE(out dataObj);
                        if (!r.Successed)
                            throw new Exception(r.ErrorMassage);
                        else
                        {
                            _dataObj = SETTourament(dataObj);
                            r = client.GET_CATEGORY(out cat);
                            if (r.Successed)
                            {
                                r = client.GET_RANKING(out rnk);
                                if (r.Successed)
                                {
                                    object[] data = new object[3] { cat, _dataObj, rnk };
                                    js = new JsonResponse() { Successed = true, session = true, data = data };
                                }
                                else
                                {
                                    throw new Exception(r.ErrorMassage);
                                }

                            }
                            else
                            {
                                throw new Exception(r.ErrorMassage);
                            }
                        }

                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                js = new JsonResponse() { Successed = false, session = true, ErrorMassage = ex.Message };
            }

            return Json(js);
        }

        public JsonResult GetEvent(TOURNAMENT dataObject, int? category)
        {

            if (category == null)
            {
                category = 1;
            }
            Result r = new Result();
            JsonResponse js = new JsonResponse();
            List<EVENT> evn = new List<EVENT>();
            List<GENDER> gn = new List<GENDER>();
            List<RANKING_DETAIL> rnk = new List<RANKING_DETAIL>();
            CATEGORY ct = new CATEGORY();
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    #region " OPERATION "
                    using (TournamentClient client = new TournamentClient())
                    {
                        r = client.GetEvent(out evn, out gn, out ct, out rnk, dataObject, category);
                        object[] obj = new object[4]
                        {
                             evn,
                             gn,
                             ct,
                             rnk
                        };
                        if (r.Successed)
                            js = new JsonResponse() { Successed = true, session = true, data = obj };
                        else
                            throw new Exception(r.ErrorMassage);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {

                js = new JsonResponse() { Successed = false, session = true, ErrorMassage = ex.Message };
            }

            return Json(js);
        }

        public JsonResult GetFirstTour()
        {
            Result r = new Result();
            JsonResponse js = new JsonResponse();
            List<EVENT> evn = new List<EVENT>();
            List<GENDER> gn = new List<GENDER>();
            CATEGORY ct = new CATEGORY();
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    #region " OPERATION "
                    var a = TempData["fTourID"];
                    js = new JsonResponse() { Successed = true, session = true, data = a };
                    #endregion
                }
            }
            catch (Exception ex)
            {

                js = new JsonResponse() { Successed = false, session = true, ErrorMassage = ex.Message };
            }

            return Json(js);
        }


        public JsonResult GetRole()
        {
            Result r = new Result();
            JsonResponse js = new JsonResponse();
            List<TT_ROLE> dataObject = new List<TT_ROLE>();
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    #region " OPERATION "
                    using (TournamentClient client = new TournamentClient())
                    {
                        r = client.GET_ROLE(out dataObject);
                        if (!r.Successed)
                            throw new Exception(r.ErrorMassage);
                        else
                            js = new JsonResponse() { Successed = true, session = true, data = dataObject };
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                js = new JsonResponse() { Successed = false, session = true, ErrorMassage = ex.Message };
            }

            return Json(js);

        }

        public JsonResult GET_USER_SEARCH(TT_USER condition)
        {
            Result r = new Result();
            JsonResponse js = new JsonResponse();
            List<DATAUSER> dataObject = new List<DATAUSER>();
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    #region " OPERATION "
                    using (TournamentClient client = new TournamentClient())
                    {
                        r = client.GET_USER_SEARCH(out dataObject, condition);
                        if (!r.Successed)
                            throw new Exception(r.ErrorMassage);
                        else
                        {
                            string user = authen();
                            var id = getIdAuthen();
                            dataObject.Where(a => a.USER_ID == user && a.ROLE_ID == 1 && a.ID == id).Select(a => { a.isAdmin = false; return a; }).ToList();
                            dataObject.Select(a => { a.BIRTH_DATE_STR = dateTostring(a.BIRTH_DATE); return a; }).ToList();
                            js = new JsonResponse() { Successed = true, session = true, data = dataObject };
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                js = new JsonResponse() { Successed = false, session = true, ErrorMassage = ex.Message };
            }

            return Json(js);
        }

        public JsonResult GetGender()
        {
            Result r = new Result();
            JsonResponse js = new JsonResponse();
            List<GENDER> dataObject = new List<GENDER>();
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    #region " OPERATION "
                    using (TournamentClient client = new TournamentClient())
                    {
                        r = client.GET_GENDER(out dataObject);
                        if (!r.Successed)
                            throw new Exception(r.ErrorMassage);
                        else
                            js = new JsonResponse() { Successed = true, session = true, data = dataObject };
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                js = new JsonResponse() { Successed = false, session = true, ErrorMassage = ex.Message };
            }

            return Json(js);

        }

        #endregion

        #region " DELETE DATA "
        public JsonResult RemoveEvent(EVENT dataObject)
        {
            Result r = new Result();
            JsonResponse js = new JsonResponse();
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    #region " OPERATION "
                    using (TournamentClient client = new TournamentClient())
                    {
                        r = client.RemoveEvent(dataObject, authen());
                        if (!r.Successed)
                            throw new Exception(r.ErrorMassage);
                        else
                            js = new JsonResponse() { Successed = true, session = true, };
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                js = new JsonResponse() { Successed = false, session = true, ErrorMassage = ex.Message };
            }

            return Json(js);
        }

        public JsonResult RemoveTour(TOURNAMENT dataObject)
        {
            Result r = new Result();
            JsonResponse js = new JsonResponse();
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    #region " OPERATION "
                    using (TournamentClient client = new TournamentClient())
                    {
                        r = client.RemoveTour(dataObject, authen());
                        if (!r.Successed)
                            throw new Exception(r.ErrorMassage);
                        else
                        {
                            js = new JsonResponse() { Successed = true, session = true, };
                            if (dataObject != null && dataObject.ID != null)
                                pathDelete(dataObject.ID);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {

                js = new JsonResponse() { Successed = false, session = true, ErrorMassage = ex.Message };
            }

            return Json(js);
        }
        public JsonResult RemoveUserAdmin(TT_USER data)
        {
            Result r = new Result();
            JsonResponse js = new JsonResponse();
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    if (data.ROLE_ID == 1)
                    {
                        var a = getIdAuthen();
                        if (data.ID == a)
                        {
                            throw new Exception("ผู้ใช้จะไม่สามารถลบสิทธิ์การเข้าใช้งานของตัวเองได้");
                        }
                    }
                    #region " OPERATION "
                    using (TournamentClient client = new TournamentClient())
                    {
                        r = client.RemoveUserAdmin(data, authen());
                        if (!r.Successed)
                            throw new Exception(r.ErrorMassage);
                        else
                            js = new JsonResponse() { Successed = true, session = true, };
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {

                js = new JsonResponse() { Successed = false, session = true, ErrorMassage = ex.Message };
            }

            return Json(js);
        }

        public JsonResult RemoveRank(RANKING data)
        {
            Result r = new Result();
            JsonResponse js = new JsonResponse();
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    #region " OPERATION "
                    using (TournamentClient client = new TournamentClient())
                    {
                        r = client.RemoveRank(data, authen());
                        if (!r.Successed)
                            throw new Exception(r.ErrorMassage);
                        else
                            js = new JsonResponse() { Successed = true, session = true, };
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {

                js = new JsonResponse() { Successed = false, session = true, ErrorMassage = ex.Message };
            }
            return Json(js);
        }

        #endregion

        #region " FILE "
        public JsonResult Logo(DataFile[] data)
        {
            JsonResponse js = new JsonResponse();
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    if (data != null && data.Length > 0)
                        manageLogo(data);
                    js = new JsonResponse() { Successed = true, ErrorMassage = null };
                }
            }
            catch (Exception ex)
            {
                js = new JsonResponse() { Successed = true, session = true, ErrorMassage = ex.Message };
            }
            return Json(js);
        }

        public JsonResult Logo_Edit(DataFile[] data)
        {
            JsonResponse js = new JsonResponse();
            int? tourID = (int?)TempData["tour_id"];
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    var dt = getLogo(data, tourID);
                    js = new JsonResponse() { Successed = true, session = true, data = dt, ErrorMassage = null };
                }
            }
            catch (Exception ex)
            {
                js = new JsonResponse() { Successed = true, session = true, ErrorMassage = ex.Message };
            }
            return Json(js);
        }

        public JsonResult RemoveLogo(DataFile data)
        {
            JsonResponse js = new JsonResponse();
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    if (data != null)
                        removeLogoFile(data);
                    js = new JsonResponse() { Successed = true, session = true, ErrorMassage = null };
                }
            }
            catch (Exception ex)
            {
                js = new JsonResponse() { Successed = true, session = true, ErrorMassage = ex.Message };
            }
            return Json(js);
        }

        public JsonResult SetData(int? data)
        {
            JsonResponse js = new JsonResponse();
            DataFile[] dtf = new DataFile[0];
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    if (data != null)
                        TempData["tour_id"] = data;
                    TempData.Remove("DataFile");
                    var dt = getLogo(dtf, data);
                    js = new JsonResponse() { Successed = true, session = true, ErrorMassage = null };
                }
            }
            catch (Exception ex)
            {
                js = new JsonResponse() { Successed = true, session = true, ErrorMassage = ex.Message };
            }
            return Json(js);
        }

        public ActionResult Upload(FormCollection formCollection, long? RANKING_ID)
        {
            JsonResponse js = new JsonResponse();
            Result r = new Result();
            RANKING_DETAIL[] rank_dt = new RANKING_DETAIL[0];
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    if (Request != null)
                    {
                        HttpPostedFileBase file = Request.Files["UploadedFile"];
                        if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                        {
                            string fileName = file.FileName;
                            string fileContentType = file.ContentType;
                            byte[] fileBytes = new byte[file.ContentLength];
                            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                            Stream stream = new MemoryStream(fileBytes);
                            IExcelDataReader reader;
                            reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);
                            var conf = new ExcelDataSetConfiguration
                            {
                                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                                {
                                    UseHeaderRow = true
                                }
                            };
                            var dataSet = reader.AsDataSet(conf);
                            using (TournamentClient client = new TournamentClient())
                            {
                                r = client.AddRanking(dataSet, RANKING_ID);
                                if (!r.Successed)
                                {
                                    throw new Exception(r.ErrorMassage);
                                }
                                TempData["ExcelError"] = "true";
                            }
                        }
                    }
                    js = new JsonResponse() { Successed = true, session = true, ErrorMassage = null };
                }
            }
            catch (Exception ex)
            {
                TempData["ExcelError"] = ex.Message;
                js = new JsonResponse() { Successed = true, session = true, ErrorMassage = ex.Message };
            }
            // return Json(js);
            return RedirectToAction("ExcelUpLoad");
        }

        public ActionResult CreateRankingID(RANKING data)
        {
            JsonResponse js = new JsonResponse();
            Result r = new Result();
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {

                    data.CREATE_BY = getIdAuthen().ToString();
                    using (TournamentClient client = new TournamentClient())
                    {
                        long? rankingID = null;
                        r = client.AddRankingID(out rankingID, data);
                        if (!r.Successed)
                        {
                            throw new Exception(r.ErrorMassage);
                        }
                    }
                    js = new JsonResponse() { Successed = true, session = true, ErrorMassage = null };
                }
            }
            catch (Exception ex)
            {
                js = new JsonResponse() { Successed = true, session = true, ErrorMassage = ex.Message };
            }
            return Json(js);
            // return RedirectToAction("ExcelUpLoad");
        }
        #endregion

        #region " UPDATE "
        public JsonResult UpdateTournament(_TOURNAMENT dataObject)
        {
            Result rs = new Result() { Successed = true };
            JsonResponse js = new JsonResponse() { data = null, session = true, Successed = true };
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    DataFile[] files = (DataFile[])TempData["DataFile"];
                    #region " OPERATION "
                    using (TournamentClient client = new TournamentClient())
                    {
                        dataObject.MODIFY_BY = authen();
                        rs = client.UpdateTournament(dataObject, ref files);
                        if (rs.Successed)
                        {
                            TempData["DataFile"] = files;
                            manageFile(dataObject.ID, client);
                            js = new JsonResponse() { Successed = true, session = true, data = null };
                            TempData["tour_id"] = dataObject.ID;
                        }
                        else
                        {
                            throw new Exception(rs.ErrorMassage);
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {

                js = new JsonResponse() { Successed = false, session = true, ErrorMassage = ex.Message };
            }

            return Json(js);
        }

        public JsonResult UpdateUser(DATAUSER data)
        {
            Result r = new Result();
            JsonResponse js = new JsonResponse();
            try
            {
                if (!IsLogin())
                {
                    js = new JsonResponse() { Successed = true, session = false };
                }
                else
                {
                    #region " OPERATION "
                    using (TournamentClient client = new TournamentClient())
                    {
                        string userLogin = authen();
                        r = client.UpdateUser(data, userLogin);
                        if (!r.Successed)
                            throw new Exception(r.ErrorMassage);
                        else
                            js = new JsonResponse() { Successed = true, session = true };
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                js = new JsonResponse() { Successed = false, session = true, ErrorMassage = ex.Message };
            }

            return Json(js);

        }
        #endregion

    }
}