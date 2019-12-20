using SPUtility;
using System;
using System.Collections.Generic;
using System.Data;
using TableTennis.BusinessLogic;
using TableTennis.DataModels;

namespace TableTennis.DataClient
{
    public partial class TournamentClient : IDisposable
    {
        #region " เพิ่มข้อมูล "
        public Result Add(out int? tour_id, _TOURNAMENT dataObj)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            tour_id = null;
            try
            {
                logic.Add(out tour_id, dataObj);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }
        public Result ADD_USER_ADMIN(TT_USER condition)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();

            try
            {
                logic.ADD_USER_ADMIN(condition);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }
        public Result AddLogo(DataFile dataObj, int? tour_id)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            try
            {
                logic.ADD_LOGO(dataObj, tour_id);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }
        public Result AddRanking( DataSet data,long? RankingID)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            try
            {
                logic.Add_Rangk(data, RankingID);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }
        public Result AddRankingID(out long? RankingID, RANKING dataobject)
        {
            Result r = new Result() { Successed = true };
            RankingID = null;
            TournamentLogic logic = new TournamentLogic();
            try
            {
                logic.Add_Rangk_ID(out RankingID, dataobject);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result AddEvent(int? TOUR_ID, EVENT[] dataObj, int? category, int? num)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            try
            {
                logic.AddEvent(TOUR_ID, dataObj, category, num);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }
        #endregion

        #region " ดึงข้อมูล "
        public Result GET_RANK_DT(out List<RANKING_DETAIL> dataObjects,long? rankID)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            dataObjects = null;
            try
            {
                logic.GET_RANK_DT(out dataObjects, rankID);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }
        public Result GET_RANK_DETAIL(out List<RANKING_DETAIL> dataObjects, long? rankID,string rankcode)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            dataObjects = null;
            try
            {
                logic.GET_RANK_DETAIL(out dataObjects, rankID,rankcode);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }
        
        public Result GET_RANKING(out List<RANKING> dataObjects)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            dataObjects = null;
            try
            {
                logic.GET_RANKING(out dataObjects);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }
        public Result GET_CATEGORY(out List<CATEGORY> dataObjects)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            dataObjects = null;
            try
            {
                logic.GET_CATEGORY(out dataObjects);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result GET_TOUR_CODE(out List<TOURNAMENT> dataObjects)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            dataObjects = null;
            try
            {
                logic.GET_TOUR_CODE(out dataObjects);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }
        public Result GET_ROLE(out List<TT_ROLE> dataObjects)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            dataObjects = null;
            try
            {
                logic.GET_ROLE(out dataObjects);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result GET_GENDER(out List<GENDER> dataObjects)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            dataObjects = null;
            try
            {
                logic.GET_GENDER(out dataObjects);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result GetEvent(out List<EVENT> evn, out List<GENDER> gn, out CATEGORY ct, out List<RANKING_DETAIL> rank,TOURNAMENT dataObject, int? cate)
        {
            evn = null;
            gn = null;
            ct = null;
            rank = null;
            Result r = new Result();
            TournamentLogic logic = new TournamentLogic();
            try
            {
                logic.GetEvent(out evn, out gn, out ct,out rank, dataObject, cate);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result ValidateUser(out TT_USER dataObjects, string user, string password)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            dataObjects = null;
            try
            {
                logic.ValidateUser(out dataObjects, user, password);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result GET_MENU(out List<TT_MENU> dataObjects, int role_id)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            dataObjects = null;
            try
            {
                logic.GET_MENU(out dataObjects, role_id);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result GET_ACCESS_TYPE(out List<TT_ACCESS_TYPE> dataObjects, int role_id)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            dataObjects = null;
            try
            {
                logic.GET_ACCESS_TYPE(out dataObjects, role_id);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result LOGIN_USER(out TT_LOGIN dataObjects, string user, string password)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            dataObjects = null;
            try
            {
                logic.LOGIN_USER(out dataObjects, user, password);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result GET_USER_SEARCH(out List<DATAUSER> dataObj, TT_USER condition)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            dataObj = null;
            try
            {
                logic.GET_USER_SEARCH(out dataObj, condition);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result GET_LOGO(out DataFile[] dataObj, int? condition)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            dataObj = null;
            try
            {
                logic.GET_LOGO(out dataObj, condition);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }
            return r;
        }


        #endregion

        #region " ลบข้อมูล "
        public Result RemoveEvent(EVENT dataObject ,string userID)
        {
            Result r = new Result();
            TournamentLogic logic = new TournamentLogic();
            try
            {
                logic.RemoveEvent(dataObject, userID);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result RemoveTour(TOURNAMENT dataObject,string userID)
        {
            Result r = new Result();
            TournamentLogic logic = new TournamentLogic();
            try
            {
                logic.RemoveTour(dataObject, userID);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }
        
        public Result RemoveUserAdmin(TT_USER dataObject,string userID)
        {
            Result r = new Result();
            TournamentLogic logic = new TournamentLogic();
            try
            {
                logic.RemoveUserAdmin(dataObject, userID);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result RemoveRank(RANKING dataObject,string userId)
        {
            Result r = new Result();
            TournamentLogic logic = new TournamentLogic();
            try
            {
                logic.RemoveRank(dataObject,userId);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        #endregion

        #region " อัพเดทข้อมูล "
        public Result UpdateTournament(_TOURNAMENT dataObject,ref DataFile[] files)
        {
            Result r = new Result();
            TournamentLogic logic = new TournamentLogic();
            try
            {
                logic.UpdateTournament(dataObject,ref files);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }
            return r;
        }

        public Result UpdateUser(DATAUSER dataObject,string userlogin)
        {
            Result r = new Result();
            TournamentLogic logic = new TournamentLogic();
            try
            {
                logic.UpdateUser(dataObject,userlogin);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }
            return r;
        }


        #endregion
    }
}
