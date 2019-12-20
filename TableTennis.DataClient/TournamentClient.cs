using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPUtility;
using TableTennis.BusinessLogic;
using TableTennis.DataModels;

namespace TableTennis.DataClient
{
    public partial class TournamentClient: IDisposable
    {
        public void Dispose()
        {
        }

        public Result Gets(out List<TOURNAMENT> dataObj,int[] v =null)
        {
            dataObj = null;
            Result r = new Result();
            TournamentLogic logic = new TournamentLogic();
            try
            {
                dataObj = logic.Gets(v);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result Get(out TOURNAMENT dataObj,int? tourId)
        {
            dataObj = null;
            Result r = new Result();
            TournamentLogic logic = new TournamentLogic();
            try
            {
                dataObj = logic.Get(tourId);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result GetCatMapTours(out List<CAT_MAP_TOUR> dataObjs, int? tourId)
        {
            dataObjs = null;
            Result r = new Result();
            TournamentLogic logic = new TournamentLogic();
            try
            {
                dataObjs = logic.GetCatMapTours(tourId);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result GetEvent(out List<EVENT> dataObj,int? id,int? catId)
        {
            dataObj = null;
            Result r = new Result();
            TournamentLogic logic = new TournamentLogic();
            try
            {
                dataObj = logic.GetEvent(id, catId);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result AddTourRegister(ref TOUR_REGISTER dataObj)
        {            
            Result r = new Result();
            TournamentLogic logic = new TournamentLogic();
            try
            {
                logic.AddTourRegister(ref dataObj);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }
            return r;
        }

        public Result GetTourRegister(out List<TOUR_REGISTER_VIEW> dataObj, int? managerId,int? tourId,int? catId)
        {
            dataObj = null;
            Result r = new Result();
            TournamentLogic logic = new TournamentLogic();
            try
            {
                dataObj = logic.GetTourRegister(managerId, tourId, catId);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }
            return r;
        }

        public Result RemovePlayer(int? regId)
        {
            Result r = new Result();
            TournamentLogic logic = new TournamentLogic();
            try
            {
                logic.RemovePlayer(regId);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }
            return r;
        }

        public Result RemoveTeam(string team_ref)
        {
            Result r = new Result();
            TournamentLogic logic = new TournamentLogic();
            try
            {
                logic.RemoveTeam(team_ref);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }
            return r;
        }

        public Result GetTourResult(out List<TOUR_REGISTER_VIEW> dataObj, ParamTourResult param, int? tourId,int? catId)
        {
            dataObj = null;
            Result r = new Result();
            TournamentLogic logic = new TournamentLogic();
            try
            {
                dataObj = logic.GetTourResult(param, tourId, catId);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }
            return r;
        }

        public Result HasRegisted(int? id, int? managerId, int? tourId ,int? catId,int? evenId)
        {
            Result r = new Result() { Successed = true };
            TournamentLogic logic = new TournamentLogic();
            try
            {
                logic.HasRegisted(id, managerId, tourId, catId, evenId);
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }

        public Result GetSlTeam(out List<SL_TEAM> slTeam, int? tourId, int? catId)
        {
            slTeam = null;
            Result r = new Result();
            TournamentLogic logic = new TournamentLogic();
            try
            {
                slTeam = logic.GetSlTeam(tourId, catId);
                r.Successed = true;
            }
            catch (Exception ex)
            {
                r.Successed = false;
                r.ErrorMassage = ex.Message;
            }

            return r;
        }
    }
}
