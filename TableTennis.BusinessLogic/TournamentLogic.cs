using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTennis.DataAccess;
using TableTennis.DataModels;

namespace TableTennis.BusinessLogic
{
    public partial class TournamentLogic: SPConnectionBase
    {       
        public List<TOURNAMENT> Gets(int[] v)
        {
            List<TOURNAMENT> dataObj = new List<TOURNAMENT>();
            List<string> rimg = new List<string>();
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                dataObj = access.Gets(v);
                foreach (var item in dataObj)
                {
                    item.img = new List<string>();
                    DataFile[] file = new DataFile[0];
                    access.GET_LOGO(out file,item.ID);
                    if (file != null && file.Length > 0)
                        item.img = file.Select(x => { return x.uri_path; }).ToList();
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
            return dataObj;
        }

        public TOURNAMENT Get(int? tourId)
        {
            TOURNAMENT dataObj = new TOURNAMENT();
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                dataObj = access.Get(tourId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
            return dataObj;
        }

        public List<EVENT> GetEvent(int? id,int? catId)
        {
            List<EVENT> dataObj = new List<EVENT>();
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                dataObj = access.GetEvent(id, catId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
            return dataObj;
        }

        public List<CAT_MAP_TOUR> GetCatMapTours(int? tourId)
        {
            List<CAT_MAP_TOUR> dataObj = new List<CAT_MAP_TOUR>();
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                dataObj = access.GetCatMapTours(tourId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
            return dataObj;
        }

        public void AddTourRegister(ref TOUR_REGISTER dataObj)
        {
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                access.BeginTransaction();
                access.AddTourRegister(ref dataObj);
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

        public List<TOUR_REGISTER_VIEW> GetTourRegister(int? managerId,int? tourId,int? catId)
        {
            List<TOUR_REGISTER_VIEW> dataObj = new List<TOUR_REGISTER_VIEW>();
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                dataObj = access.GetTourRegister(managerId, tourId, catId).OrderBy(x=>x.TH_EVENT_NAME).ThenBy(x=>x.TEAM_REF).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
            return dataObj;
        }

        public void RemovePlayer(int? regId)
        {
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                access.BeginTransaction();
                access.RemovePlayer(regId);
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

        public void RemoveTeam(string team_ref)
        {
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                access.BeginTransaction();
                access.RemoveTeam(team_ref);
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

        public List<TOUR_REGISTER_VIEW> GetTourResult(ParamTourResult param, int? tourId,int? catId)
        {
            List<TOUR_REGISTER_VIEW> dataObj = new List<TOUR_REGISTER_VIEW>();
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                string[] team_ref = null;
                if (catId != 1)
                {
                    if (param.ID != null)
                    {
                        dataObj = access.GetTourResult(param, tourId, catId, null);
                        if (dataObj.Count > 0)
                        {
                            param.ID = null;
                            param.TEAM_REF = dataObj.FirstOrDefault().TEAM_REF;
                        }
                    }
                    else if (param.NAME != null)
                    {
                        dataObj = access.GetTourResult(param, tourId, catId, null);
                        if (dataObj.Count > 0)
                        {
                            param.NAME = null;
                            team_ref = dataObj.GroupBy(x => x.TEAM_REF).Select(x => x.Key).ToArray();
                        }
                    }
                }                
                dataObj = access.GetTourResult(param, tourId, catId, team_ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
            return dataObj;
        }


        public void HasRegisted(int? id, int? managerId, int? tourId,int? catId,int? evenId)
        {
            List<TOUR_REGISTER> dataObj = new List<TOUR_REGISTER>();
            EVENT dataE = new EVENT();
            PLAYER dataP = new PLAYER();
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {
                
                dataObj = access.PlayRegister(id, tourId);
                if(dataObj.Count > 0)
                {
                    dataObj = (from x in dataObj where x.MANAGER_ID == managerId select x).ToList();
                    if (dataObj.Count == 0)
                    {
                        throw new Exception("ผู้เล่นถูกสมัครในสังกัดอื่นแล้ว");
                    }
                    else
                    {
                        dataObj = (from x in dataObj where x.CATEGORY_ID == catId select x).ToList();
                        if(dataObj.Count > 0)
                        {
                            if (catId != 1)
                                dataObj = (from x in dataObj where x.TOUR_MAP_ID == evenId select x).ToList();

                            if (dataObj.Count > 0)
                                throw new Exception("ผู้เล่นสมัครการแข่งขันแล้ว");
                        }
                    }
                }
                dataE = access.tourMapEvent(evenId);
                dataP = access.GetPlayer(id);
                if (dataE!=null && dataP!=null)
                {
                    if(!string.IsNullOrEmpty(dataE.GEN_CODE)&& dataP.GENDER != null)
                    {
                        if(dataE.GEN_CODE != dataP.GENDER.ToString())
                        {
                            if (dataE.GEN_CODE == "M")
                            {
                                throw new Exception("เฉพาะเพศชายเท่านั้น");
                            }
                            else if (dataE.GEN_CODE == "F")
                            {
                                throw new Exception("เฉพาะเพศหญิงเท่านั้น");
                            }
                        }
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

        public List<SL_TEAM> GetSlTeam(int? tourId, int? catId)
        {
            List<SL_TEAM> dataObj = new List<SL_TEAM>();
            TournamentAccess access = new TournamentAccess(connnectionName);
            access.OpenConnection();
            try
            {                
                dataObj = access.GetSlTeam(tourId, catId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                access.CloseConnection();
            }
            return dataObj;
        }
    }
}
