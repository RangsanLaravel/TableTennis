using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTennis.DataAccess;
using TableTennis.DataModels;

namespace TableTennis.BusinessLogic
{
    public partial class TournamentLogic : SPConnectionBase
    {
        #region " เพิ่มข้อมูล "
        public void Add(out int? tour_id, _TOURNAMENT dataObject)
        {
            List<TOURNAMENT> dataObj = new List<TOURNAMENT>();
            TournamentAccess access = new TournamentAccess(connnectionName);
            tour_id = null;
            access.OpenConnection();
            access.BeginTransaction();
            try
            {
                dataObject.EXPIRE_ON = stringTodate(dataObject.EXPIRE_ON_STR);
                dataObject.TOUR_START_ON = stringTodate(dataObject.TOUR_START_ON_STR);
                dataObject.TOUR_END_ON = stringTodate(dataObject.TOUR_END_ON_STR);
                access.Add(out tour_id, dataObject);
                access.Commit();
            }
            catch (Exception ex)
            {
                access.Rollback();
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }
        public void ADD_USER_ADMIN(TT_USER dataObject)
        {
            List<TOURNAMENT> dataObj = new List<TOURNAMENT>();
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            access.BeginTransaction();
            try
            {
                access.ADD_USER_ADMIN(dataObject);
                access.Commit();
            }
            catch (Exception ex)
            {
                access.Rollback();
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }
        public void Add_Rangk(DataSet data, long? RankingID)
        {
            List<TOURNAMENT> dataObj = new List<TOURNAMENT>();
            TournamentAccess access = new TournamentAccess(connnectionName);
            RANKING_DETAIL[] rankdt = new RANKING_DETAIL[0];
            access.OpenConnection();
            access.BeginTransaction();
            try
            {
                if (RankingID != null)
                {
                    rankdt = manageRanking(data, RankingID);
                    if (rankdt != null && rankdt.Length > 0)
                    {
                     
                        access.ADD_Ranking(rankdt);

                    }
                    else
                    {
                        throw new Exception("format not support");
                    }
                }
                access.Commit();
            }
            catch (Exception ex)
            {
                access.Rollback();
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }
        protected RANKING_DETAIL[] manageRanking(DataSet data, long? rangkingid)
        {
            List<RANKING_DETAIL> tem = new List<RANKING_DETAIL>();
            foreach (DataTable item in data.Tables)
            {
                if (!string.IsNullOrEmpty(item.Rows[3][0] + ""))
                    continue;
                foreach (DataRow row in item.Rows)
                {
                    int n;
                    if (!int.TryParse(row[1] + "", out n))
                        continue;
                    if (!int.TryParse(row[3] + "", out n))
                        continue;
                    tem.Add(setData(row, rangkingid, item.TableName));
                }
            }
            return tem.ToArray();
        }
        private RANKING_DETAIL setData(DataRow row, long? rankID, string rank_code)
        {
            RANKING_DETAIL temrang = new RANKING_DETAIL();
            temrang.RANK_CODE = rank_code;
            temrang.PLAYER_ID = convertStrToint(row[3] + "");
            temrang.BELONG_TO = row[6] + "";
            temrang.RANK = convertStrToint(row[1] + "");
            temrang.POINT = row[7] + "";
            temrang.RANKING_ID = rankID;
            return temrang;
        }
        private int? convertStrToint(string v)
        {
            int? a = null;
            if (!string.IsNullOrEmpty(v))
            {
                int n;
                if (int.TryParse(v, out n))
                    a = Convert.ToInt32(v);
            }
            return a;
        }
        public void Add_Rangk_ID(out long? RankingId, RANKING dataObject)
        {
            List<TOURNAMENT> dataObj = new List<TOURNAMENT>();
            TournamentAccess access = new TournamentAccess(connnectionName);
            RANKING_DETAIL[] rankdt = new RANKING_DETAIL[0];
            access.OpenConnection();
            access.BeginTransaction();
            try
            {

                access.ADD_Ranking_id(out RankingId, dataObject);
                access.Commit();
            }
            catch (Exception ex)
            {
                access.Rollback();
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }
        public void ADD_LOGO(DataFile dataObject, int? tour_id)
        {
            List<TOURNAMENT> dataObj = new List<TOURNAMENT>();
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            access.BeginTransaction();
            try
            {
                access.ADD_LOGO(dataObject, tour_id);
                access.Commit();
            }
            catch (Exception ex)
            {
                access.Rollback();
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }

        public void AddEvent(int? TOUR_ID, EVENT[] dataObjects, int? category, int? num_of_ply)
        {
            List<TOURNAMENT> dataObj = new List<TOURNAMENT>();
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            access.BeginTransaction();
            try
            {
                if (dataObjects != null)
                {
                    foreach (var item in dataObjects)
                    {
                        access.AddEvent(TOUR_ID, item, category, num_of_ply);
                    }
                    access.Commit();
                }
            }
            catch (Exception ex)
            {
                access.Rollback();
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }
        #endregion

        #region " ดึงข้อมูล "

        public void GET_CATEGORY(out List<CATEGORY> dataObj)
        {
            dataObj = null;
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                dataObj = access.GET_CATEGORY();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }

        public void GET_TOUR_CODE(out List<TOURNAMENT> dataObj)
        {
            dataObj = null;
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                dataObj = access.GET_TOUR_CODE();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }
        public void GET_RANK_DT(out List<RANKING_DETAIL> dataObj, long? rankId)
        {
            dataObj = null;
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                access.GET_RANK_DT_CODE(out dataObj, rankId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }
        public void GET_RANK_DETAIL(out List<RANKING_DETAIL> dataObj, long? rankId,string rankcode)
        {
            dataObj = null;
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                access.GET_RANK_DETAIL(out dataObj, rankId, rankcode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }
        public void GET_ROLE(out List<TT_ROLE> dataObj)
        {
            dataObj = null;
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                dataObj = access.GET_ROLE();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }

        public void GET_GENDER(out List<GENDER> dataObj)
        {
            dataObj = null;
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                dataObj = access.GetGender();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }

        public void GET_RANKING(out List<RANKING> dataObj)
        {
            dataObj = null;
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                dataObj = access.GetRanking();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }
        public void GetEvent(out List<EVENT> evn, out List<GENDER> gn, out CATEGORY ct, out List<RANKING_DETAIL> rank, TOURNAMENT dataObject, int? cate)
        {
            evn = null;
            rank = null;
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            List<TOUR_MAPBASE> tmp = new List<TOUR_MAPBASE>();
            gn = null;
            try
            {
                access.GET_RANK_DT(out rank, dataObject.ID);
                evn = access.GetEvent();
                access.Get_CATE_MAP_TOUR(out ct, dataObject, cate);
                if (ct != null)
                    tmp = access.GetEventInTourMapbase(dataObject, cate);
                gn = access.GetGender();
                gn = gn.OrderByDescending(x => x.GEN_ID)
                         .ToList();
                if (evn.Count > 0 && tmp.Count > 0)
                {
                    //evn.Where(c=>c.PRICE)
                    //evn.First().GEN_CODE = "M";
                    foreach (var item in tmp)
                    {
                        evn.Where(c => c.ID == item.EVENT_ID).Select(c => { c.TOUR_SELECT = true; c.PRICE = item.PAY; return c; }).ToList();
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }

        public void ValidateUser(out TT_USER dataObj, string user, string password)
        {
            dataObj = null;
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                int n;
                bool isNumeric = int.TryParse(user, out n);
                if (isNumeric)
                    dataObj = access.ValidateUserID(user, password);
                else
                    dataObj = access.ValidateUserEmail(user, password);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }

        public void GET_MENU(out List<TT_MENU> dataObj, int? role_id)
        {
            dataObj = null;
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                dataObj = access.GET_MENU(role_id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }

        public void GET_ACCESS_TYPE(out List<TT_ACCESS_TYPE> dataObj, int? role_id)
        {
            dataObj = null;
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                dataObj = access.GET_ACCESS_TYPE(role_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }
        // GET_USER_SEARCH
        public void LOGIN_USER(out TT_LOGIN dataObj, string user, string password)
        {
            dataObj = new TT_LOGIN();
            dataObj.dataUser = new TT_USER();
            dataObj.menu_show = new List<TT_MENU>();
            dataObj.user_access = new List<TT_ACCESS_TYPE>();
            dataObj.isLogin = false;
            TT_USER temp = new TT_USER();
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                ValidateUser(out temp, user, password);
                if (temp != null)
                {
                    dataObj.dataUser = temp;
                    List<TT_MENU> tm = new List<TT_MENU>();
                    GET_MENU(out tm, dataObj.dataUser.ROLE_ID);
                    dataObj.menu_show = tm;
                    List<TT_ACCESS_TYPE> tat = new List<TT_ACCESS_TYPE>();
                    GET_ACCESS_TYPE(out tat, dataObj.dataUser.ROLE_ID);
                    dataObj.user_access = tat;
                    dataObj.isLogin = true;
                }
                else
                {
                    throw new Exception(" user หรือ password ไม่ถูกต้อง ");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }

        public void GET_USER_SEARCH(out List<DATAUSER> dataObj, TT_USER condition)
        {
            dataObj = null;
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                if (condition.ROLE_ID == 2)
                {
                    access.GET_USER_SEARCH_MANGER(out dataObj, condition);
                    dataObj.Select(a => { a.ROLE_ID = 2; return a; }).ToList();
                }
                else if (condition.ROLE_ID == 3)
                {
                    access.GET_USER_SEARCH_PLAYER(out dataObj, condition);
                    dataObj.Select(a => { a.ROLE_ID = 3; return a; }).ToList();
                }
                else
                {
                    access.GET_USER_SEARCH(out dataObj, condition);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }

        public void GET_LOGO(out DataFile[] dataObj, int? condition)
        {
            dataObj = null;
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                access.GET_LOGO(out dataObj, condition);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }

        #endregion

        #region " ลบข้อมูล "RemoveRank
        public void RemoveEvent(EVENT dataObject, string userID)
        {
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            access.BeginTransaction();
            try
            {
                if (dataObject.ID != null)
                    access.RemoveEvent(dataObject, userID);
                access.Commit();
            }
            catch (Exception ex)
            {
                access.Rollback();
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }
        public void RemoveTour(TOURNAMENT dataObject, string userID)
        {
            List<TOURNAMENT> dataObj = new List<TOURNAMENT>();
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            access.BeginTransaction();
            try
            {
                if (dataObject.ID != null)
                    access.RemoveTour(dataObject, userID);
                access.Commit();
            }
            catch (Exception ex)
            {
                access.Rollback();
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }
        public void RemoveUserAdmin(TT_USER dataObject, string userID)
        {
            List<TOURNAMENT> dataObj = new List<TOURNAMENT>();
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            access.BeginTransaction();
            try
            {
                if (dataObject.ID != null)
                    access.RemoveUserAdmin(dataObject, userID);
                else
                    throw new Exception("ID IS NULL !!");
                access.Commit();
            }
            catch (Exception ex)
            {
                access.Rollback();
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }

        public void RemoveRank(RANKING dataObject, string userId)
        {
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            access.BeginTransaction();
            try
            {
                access.RemoveRank(dataObject);
                access.UpdateRank_modify(dataObject, userId);
                access.Commit();
            }
            catch (Exception ex)
            {
                access.Rollback();
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }

        #endregion

        #region " อัพเดท "
        public void UpdateTournament(_TOURNAMENT dataObject, ref DataFile[] files)
        {
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            access.BeginTransaction();
            DataFile[] tempFile = new DataFile[0];
            List<DataFile> dt = new List<DataFile>();
            List<DataFile> tdt = new List<DataFile>();
            try
            {
                dataObject.EXPIRE_ON = stringTodate(dataObject.EXPIRE_ON_STR);
                dataObject.TOUR_START_ON = stringTodate(dataObject.TOUR_START_ON_STR);
                dataObject.TOUR_END_ON = stringTodate(dataObject.TOUR_END_ON_STR);
                access.UpdateTournament(dataObject);

                access.GET_LOGO(out tempFile, dataObject.ID);

                #region " MANAGE FILE "
                if (files != null && files.Length > 0)
                {
                    dt = tempFile.ToList();
                    foreach (var item in files)
                    {
                        dt.RemoveAll(a => a.image_id == item.image_id);
                    }
                    if (dt.Count > 0)
                        foreach (var item in dt)
                        {
                            access.RemoveFile(item, dataObject.MODIFY_BY);
                            // deleteFile(item.path_file);
                        }
                    tdt = files.ToList();
                    tdt.RemoveAll(a => a.image_id != null);
                    foreach (var item in tempFile)
                    {
                        tdt.RemoveAll(a => a.name == item.name && a.size == item.size);
                    }
                    files = tdt.ToArray();
                }
                else
                {
                    if (tempFile != null && tempFile.Length > 0)
                    {
                        foreach (var item in tempFile)
                        {
                            access.RemoveFile(item, dataObject.MODIFY_BY);
                            // deleteFile(item.path_file);
                        }
                    }
                }
                #endregion

                access.Commit();
            }
            catch (Exception ex)
            {
                access.Rollback();
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }

        public void UpdateUser(DATAUSER dataObject, string userlogin)
        {
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            access.BeginTransaction();
            try
            {
                if (dataObject.ROLE_ID != null)
                {
                    dataObject.BIRTH_DATE = stringTodate(dataObject.BIRTH_DATE_STR);
                    access.UpdateUser(dataObject, userlogin);
                }
                else
                {
                    throw new Exception("ไม่พบกลุ่มผู้ใช้งาน");
                }
                access.Commit();
            }
            catch (Exception ex)
            {
                access.Rollback();
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
        }
        private DateTime? stringTodate(string eXPIRE_ON_STR)
        {
            System.Globalization.CultureInfo _cultureTHInfo = new System.Globalization.CultureInfo("th-TH");
            DateTime? dt = null;
            if (!string.IsNullOrEmpty(eXPIRE_ON_STR))
                dt = DateTime.ParseExact(eXPIRE_ON_STR, "dd/MM/yyyy", _cultureTHInfo);
            return dt;
        }

        private void deleteFile(string path)
        {
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }
        #endregion
    }
}
