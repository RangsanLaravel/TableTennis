using DataAccessUtility;
using MySql.Data.MySqlClient;
using SPUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableTennis.DataModels;

namespace TableTennis.DataAccess
{
    public partial class TournamentAccess:RepositoryBase
    {
        public TournamentAccess(string connectionName)
        {
            base.Repository(connectionName);
        }

        public List<TOURNAMENT> Gets(int[] v)
        {
            StringBuilder sqlSb = new StringBuilder();            
            sqlSb.AppendLine(@"
                               SELECT t.*,
                                CASE WHEN t.EXPIRE_ON >=curdate() then 'TRUE' else 'FALSE' end 'EXPRIE'
                                FROM tournament  t
                               INNER JOIN (SELECT TOURNAMENT_ID FROM tour_mapbase GROUP BY TOURNAMENT_ID)  tm 
                               ON t.ID = tm.TOURNAMENT_ID
                               where t.TENINATE_FLG='N' AND t.TOUR_END_ON >= curdate()
                                ");
            return Utility.ExecuteToList<TOURNAMENT>(sqlSb.ToString(), connection);
        }

        public TOURNAMENT Get(int? tourId)
        {
            StringBuilder sqlSb = new StringBuilder();
            List<MySqlParam> param = new List<MySqlParam>();
            StringBuilder condition = new StringBuilder();
            if (tourId != null)
            {
                condition.AppendLine(@" AND ID=@vID");
                param.Add(new MySqlParam("vID", tourId,MySqlDbType.Int32));
            }

            sqlSb.AppendLine("SELECT * ,CASE WHEN EXPIRE_ON >curdate() then 'TRUE' else 'FALSE' end 'EXPRIE' FROM tournament where TENINATE_FLG='N'");
            sqlSb.AppendLine(condition.ToString());
            return Utility.ExecuteToList<TOURNAMENT>(sqlSb.ToString(), connection,param).FirstOrDefault();
        }

        public List<EVENT> GetEvent(int? id,int? catId)
        {
            StringBuilder sqlSb = new StringBuilder();
            List<MySqlParam> param = new List<MySqlParam>();
            StringBuilder condition = new StringBuilder();
            if (id != null)
            {
                condition.AppendLine(@" AND  tm.TOURNAMENT_ID=@vTOURNAMENT_ID");
                param.Add(new MySqlParam("vTOURNAMENT_ID", id, MySqlDbType.Int32));
            }

            if (id != null)
            {
                condition.AppendLine(@" AND  tm.CAT_ID=@vCAT_ID");
                param.Add(new MySqlParam("vCAT_ID", catId, MySqlDbType.Int32));
            }

            sqlSb.AppendLine(@"select tm.ID, e.TH_EVENT_NAME from event e
                            inner join tour_mapbase tm on e.ID = tm.EVENT_ID and e.TENINATE_FLG = 'N'
                            where tm.TENINATE_FLG = 'N' ");
            sqlSb.AppendLine(condition.ToString());

            return Utility.ExecuteToList<EVENT>(sqlSb.ToString(), connection, param);
        }

        public List<CAT_MAP_TOUR> GetCatMapTours(int? tourId)
        {
            StringBuilder sqlSb = new StringBuilder();
            List<MySqlParam> param = new List<MySqlParam>();

            param.Add(new MySqlParam("vTOUR_ID", tourId, MySqlDbType.Int32));

            sqlSb.AppendLine(@"select c.DESCRIPTION,cmt.* from table_tennis.cat_map_tour cmt
                                inner join table_tennis.category c on cmt.cat_id = c.id
                                where tour_id =@vTOUR_ID");

            return Utility.ExecuteToList<CAT_MAP_TOUR>(sqlSb.ToString(), connection, param);
        }

        public void AddTourRegister(ref TOUR_REGISTER dataObj)
        {
            MySqlCommand cmd = new MySqlCommand(@"ADD_TOUR_REGISTER", connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@vTOUR_MAP_ID", dataObj.TOUR_MAP_ID);
            cmd.Parameters["@vTOUR_MAP_ID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@vMANAGER_ID", dataObj.MANAGER_ID);
            cmd.Parameters["@vMANAGER_ID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@vPLAYER_ID", dataObj.PLAYER_ID);
            cmd.Parameters["@vPLAYER_ID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@vCREATE_BY", dataObj.CREATE_BY);
            cmd.Parameters["@vCREATE_BY"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@vCATEGORY_ID", dataObj.CATEGORY_ID);
            cmd.Parameters["@vCATEGORY_ID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@vTEAM_REF",dataObj.TEAM_REF);
            cmd.Parameters["@vTEAM_REF"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@vGROUP_NAME", dataObj.GROUP_NAME);
            cmd.Parameters["@vGROUP_NAME"].Direction = ParameterDirection.Input;
            
            cmd.Parameters.AddWithValue("@vID", MySqlDbType.Int64);
            cmd.Parameters["@vID"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            dataObj.ID = (int)cmd.Parameters["@vID"].Value;
        }

        public List<TOUR_REGISTER_VIEW> GetTourRegister(int? managerId,int? tourId,int? catId)
        {
            StringBuilder sqlSb = new StringBuilder();
            List<MySqlParam> param = new List<MySqlParam>();
            StringBuilder condition = new StringBuilder();
            if (managerId != null)
            {
                condition.AppendLine(@" and m.id =@vMANAGER_ID");
                param.Add(new MySqlParam("vMANAGER_ID", managerId, MySqlDbType.Int32));
            }

            if (tourId != null)
            {
                condition.AppendLine(@"and tm.tournament_id = @vTOURNAMENT_ID");
                param.Add(new MySqlParam("vTOURNAMENT_ID", tourId, MySqlDbType.Int32));

            }

            if (catId != null)
            {
                condition.AppendLine(@"and tr.CATEGORY_ID = @vCATEGORY_ID");
                param.Add(new MySqlParam("vCATEGORY_ID", catId, MySqlDbType.Int32));        
            }

            sqlSb.AppendLine(@"SELECT p.ID,p.FIRST_NAME,p.LAST_NAME,p.BIRTH_DATE,p.GENDER,e.TH_EVENT_NAME,tm.PAY,m.TEAM ,tr.ID as REG_ID,tr.TEAM_REF,tr.GROUP_NAME,CASE WHEN t.EXPIRE_ON >curdate() then 'TRUE' else 'FALSE' end 'EXPRIE',rd.RANK ,rd.POINT
                                FROM table_tennis.tour_register tr
                                inner join table_tennis.player p on tr.player_id = p.id
                                inner join table_tennis.manager m on tr.manager_id = m.id
                                inner join table_tennis.tour_mapbase tm on tr.tour_map_id = tm.id
                                inner join table_tennis.tournament t on tm.tournament_id = t.id
                                inner join table_tennis.event e on tm.event_id = e.id 
                                left join table_tennis.rank_detail rd on  t.ranking_id = rd.ranking_id and tr.player_id=rd.player_id and e.code = rd.event_code
                                where tr.TENINATE_FLG='N'  ");
            sqlSb.AppendLine(condition.ToString());

            return Utility.ExecuteToList<TOUR_REGISTER_VIEW>(sqlSb.ToString(), connection, param);
        }

        public List<TOUR_REGISTER_VIEW> GetTourResult(ParamTourResult param, int? tourId,int? catId,string[] team_ref)
        {
            StringBuilder sqlSb = new StringBuilder();
            List<MySqlParam> _param = new List<MySqlParam>();
            StringBuilder condition = new StringBuilder();

            if(param.ID != null)
            {
                condition.AppendLine("and tr.player_id = @vID");
                _param.Add(new MySqlParam("vID", param.ID, MySqlDbType.Int64));
            }

            if(param.EVENT_ID != null)
            {
                condition.AppendLine("and tm.ID = @vEVENT_ID");
                _param.Add(new MySqlParam("vEVENT_ID", param.EVENT_ID, MySqlDbType.Int32));
            }

            if (!string.IsNullOrEmpty(param.NAME))
            {
                condition.AppendLine("and CONCAT(p.FIRST_NAME,' ',p.LAST_NAME) like @vNAME ");
                _param.Add(new MySqlParam("vNAME", "%"+param.NAME+"%", MySqlDbType.VarChar));
            }

            if (!string.IsNullOrEmpty(param.TEAM))
            {
                condition.AppendLine("and m.ID = @vTEAM");
                _param.Add(new MySqlParam("vTEAM", param.TEAM, MySqlDbType.Int64));
            }

            if (tourId != null)
            {
                condition.AppendLine(@"and tm.tournament_id = @vTOURNAMENT_ID");
                _param.Add(new MySqlParam("vTOURNAMENT_ID", tourId, MySqlDbType.Int32));
            }

            if(catId != null)
            {
                condition.AppendLine("and tr.CATEGORY_ID = @vCATEGORY_ID ");
                _param.Add(new MySqlParam("vCATEGORY_ID", catId, MySqlDbType.Int32));

            }

            if (!string.IsNullOrEmpty(param.TEAM_REF))
            {
                condition.AppendLine("and tr.TEAM_REF = @vTEAM_REF ");
                _param.Add(new MySqlParam("vTEAM_REF", param.TEAM_REF, MySqlDbType.VarChar));
            }

            if (team_ref != null)
            {
                condition.Append(" AND tr.TEAM_REF in (");
                for(int i = 0; i < team_ref.Length; i++)
                {
                    if (i != team_ref.Length - 1)
                    {
                        condition.Append(team_ref[i] + ",");
                    }
                    else
                    {
                        condition.Append(team_ref[i]);
                    }                    
                }
                condition.Append(")");
            }

            sqlSb.AppendLine(@"SELECT p.ID,p.FIRST_NAME,p.LAST_NAME,p.BIRTH_DATE,p.GENDER,e.TH_EVENT_NAME,tm.PAY,m.TEAM ,tr.ID as REG_ID,tr.TEAM_REF,tr.GROUP_NAME,rd.RANK ,rd.POINT
                                FROM table_tennis.tour_register tr
                                inner join table_tennis.player p on tr.player_id = p.id
                                inner join table_tennis.manager m on tr.manager_id = m.id
                                inner join table_tennis.tour_mapbase tm on tr.tour_map_id = tm.id
                                inner join table_tennis.event e on tm.event_id = e.id
                                inner join table_tennis.tournament t on tm.tournament_id = t.id
                                left join table_tennis.rank_detail rd on  t.ranking_id = rd.ranking_id and tr.player_id=rd.player_id and e.code = rd.event_code
                                where tr.TENINATE_FLG='N' ");
            sqlSb.AppendLine(condition.ToString());            
            return Utility.ExecuteToList<TOUR_REGISTER_VIEW>(sqlSb.ToString(), connection, _param);
        }

        public void RemovePlayer(int? regId)
        {
            MySqlCommand cmd = new MySqlCommand(@"delete from  table_tennis.tour_register where id =@vID",connection);
            cmd.Parameters.Add("vID", MySqlDbType.Int32).Value = regId;
            cmd.ExecuteNonQuery();
        }

        public void RemoveTeam(string team_ref)
        {
            MySqlCommand cmd = new MySqlCommand(@"delete from  table_tennis.tour_register where TEAM_REF =@vTEAM_REF", connection);
            cmd.Parameters.Add("vTEAM_REF", MySqlDbType.VarChar).Value = team_ref;
            cmd.ExecuteNonQuery();
        }

        public List<TOUR_REGISTER> PlayRegister(int? id, int? tourId)
        {
            StringBuilder sqlSb = new StringBuilder();
            List<MySqlParam> _param = new List<MySqlParam>();
            StringBuilder condition = new StringBuilder();

            if (tourId != null)
            {
                condition.AppendLine(@"and tm.tournament_id = @vTOURNAMENT_ID");
                _param.Add(new MySqlParam("vTOURNAMENT_ID", tourId, MySqlDbType.Int32));

            }

            if (id != null)
            {
                condition.AppendLine(@"and tr.PLAYER_ID = @vPLAYER_ID");
                _param.Add(new MySqlParam("vPLAYER_ID", id, MySqlDbType.Int32));

            }

            sqlSb.AppendLine(@"SELECT tr.* FROM table_tennis.tour_register tr 
                                INNER JOIN table_tennis.tour_mapbase tm ON tr.tour_map_id = tm.id
                                WHERE tr.TENINATE_FLG='N'
                            ");

            sqlSb.AppendLine(condition.ToString());

            return Utility.ExecuteToList<TOUR_REGISTER>(sqlSb.ToString(), connection, _param);
        }

        public EVENT tourMapEvent(int? evenId)
        {
            StringBuilder sqlSb = new StringBuilder();
            List<MySqlParam> _param = new List<MySqlParam>();
            sqlSb.AppendLine(@"SELECT e.* FROM tour_mapbase tm 
                            INNER JOIN EVENT e ON tm.EVENT_ID = e.ID AND tm.TENINATE_FLG ='N' WHERE tm.id =@vID");
            _param.Add(new MySqlParam("vID", evenId, MySqlDbType.Int32));
            return Utility.ExecuteToList<EVENT>(sqlSb.ToString(), connection,_param).FirstOrDefault();
        }
        public PLAYER GetPlayer(int? id)
        {
            if (id == null)
                return null;

            PLAYER dataObj = new PLAYER();
            StringBuilder sqlSb = new StringBuilder();
            List<MySqlParam> param = new List<MySqlParam>();
            param.Add(new MySqlParam("ID", id, MySqlDbType.Int64));
            sqlSb.AppendLine("SELECT * FROM player WHERE ID =@ID");
            return Utility.ExecuteToList<PLAYER>(sqlSb.ToString(), connection, param).FirstOrDefault();
        }

        public List<SL_TEAM> GetSlTeam(int? tourId, int? catId)
        {
            StringBuilder sqlSb = new StringBuilder();
            List<MySqlParam> _param = new List<MySqlParam>();
            _param.Add(new MySqlParam("vTOUR_ID", tourId, MySqlDbType.Int64));
            _param.Add(new MySqlParam("vCAT_ID", catId, MySqlDbType.Int64));
            if (catId == 1)
            {
                sqlSb.AppendLine(@"
select tr.MANAGER_ID ID
,m.TEAM AS 'DESC'
,count(tr.manager_id) AMOUNT
from tour_register tr
inner join manager m on tr.manager_id = m.id 
inner join tour_mapbase tm on tr.tour_map_id = tm.id
where tm.cat_id = @vCAT_ID and tm.tournament_id = @vTOUR_ID
GROUP BY tr.MANAGER_ID,m.TEAM
");
            }
            else
            {
                sqlSb.AppendLine(@"
select tr.MANAGER_ID ID
,m.TEAM AS 'DESC'
,count(tr.TEAM_REF) AMOUNT
from (
SELECT tour_map_id,manager_id,TEAM_REF FROM tour_register
where team_ref is not null
group by tour_map_id,manager_id,team_ref
) tr
inner join manager m on tr.manager_id = m.id 
inner join tour_mapbase tm on tr.tour_map_id = tm.id
where tm.cat_id = @vCAT_ID and tm.tournament_id = @vTOUR_ID
GROUP BY tr.MANAGER_ID,m.TEAM
");
            }
            
            return Utility.ExecuteToList<SL_TEAM>(sqlSb.ToString(), connection, _param);
        }
    }
}
