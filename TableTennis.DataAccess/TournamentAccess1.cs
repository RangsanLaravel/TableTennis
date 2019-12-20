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
    public partial class TournamentAccess : RepositoryBase
    {
        #region " เพิ่มข้อมูล "
        public void Add(out int? tour_id, _TOURNAMENT dataObject)
        {
            tour_id = null;
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"INSERT INTO table_tennis.tournament(
                                CODE, 
                                EN_TOUR_NAME,
                                TH_TOUR_NAME,
                                DESCRIPTION, 
                                TOUR_POINT,
                                 TOUR_ADDRESS,
                                CREATE_ON, 
                                CREATE_BY, 
                                MODIFY_ON,
                                MODIFY_BY, 
                                EXPIRE_ON, 
                                TENINATE_FLG,
                                 TOUR_START_ON,
                                 TOUR_END_ON,
                                RANKING_ID
                                )
                                VALUES
                                (?CODE,
                                ?EN_TOUR_NAME,
                                ?TH_TOUR_NAME,
                                ?DESCRIPTION,
                                ?TOUR_POINT,
                                ?TOUR_ADDRESS,
                                SYSDATE(),
                                ?CREATE_BY,
                                ?MODIFY_ON,
                                ?MODIFY_BY,
                                ?EXPIRE_ON,
                                'N',
                                ?TOUR_START_ON,
                                ?TOUR_END_ON,
                                ?RANKING_ID
                                );select last_insert_id();";
            //cmd.Parameters.Add(new MySqlParameter("", dataObject));
            cmd.Parameters.Add(new MySqlParameter("?CODE", dataObject.CODE));
            cmd.Parameters.Add(new MySqlParameter("?EN_TOUR_NAME", dataObject.EN_TOUR_NAME));
            cmd.Parameters.Add(new MySqlParameter("?TH_TOUR_NAME", dataObject.TH_TOUR_NAME));
            cmd.Parameters.Add(new MySqlParameter("?DESCRIPTION", dataObject.DESCRIPTION));

            cmd.Parameters.Add(new MySqlParameter("?TOUR_POINT", dataObject.TOUR_POINT));
            cmd.Parameters.Add(new MySqlParameter("?TOUR_ADDRESS", dataObject.TOUR_ADDRESS));

            cmd.Parameters.Add(new MySqlParameter("?CREATE_BY", dataObject.CREATE_BY));
            cmd.Parameters.Add(new MySqlParameter("?MODIFY_ON", dataObject.MODIFY_ON));
            cmd.Parameters.Add(new MySqlParameter("?MODIFY_BY", dataObject.MODIFY_BY));
            cmd.Parameters.Add(new MySqlParameter("?EXPIRE_ON", dataObject.EXPIRE_ON));
            cmd.Parameters.Add(new MySqlParameter("?TENINATE_FLG", dataObject.TENINATE_FLG));
            cmd.Parameters.Add(new MySqlParameter("?TOUR_START_ON", dataObject.TOUR_START_ON));
            cmd.Parameters.Add(new MySqlParameter("?TOUR_END_ON", dataObject.TOUR_END_ON));
            cmd.Parameters.Add(new MySqlParameter("?RANKING_ID", dataObject.RANKING_ID));
            //cmd.Parameters.Add("?address", MySqlDbType.VarChar).Value = "myaddress";
            // cmd.ExecuteNonQuery();
            tour_id = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.Dispose();
        }

        public void ADD_USER_ADMIN(TT_USER condition)
        {
            MySqlCommand cmd = new MySqlCommand();
            List<TT_MENU> result = new List<TT_MENU>();
            cmd.Connection = connection;
            cmd.CommandText = @"INSERT INTO `table_tennis`.`tt_user`
                             ( USER_ID,                           
                              `USER_NAME`,
                              `USER_SURNAME`,
                              `EMAIL`,
                              `PASSWORD`,
                              `ROLE_ID`,
                              `TMN_DT`)
                        VALUES ( 
                                ?vUSER_ID,
                                ?vUSER_NAME,
                                ?vUSER_SURNAME,
                                ?vEMAIL,
                                ?vPASSWORD,
                                ?vROLE_ID,
                                null) ";
            cmd.Parameters.Add(new MySqlParameter("?vUSER_ID", condition.USER_ID));
            cmd.Parameters.Add(new MySqlParameter("?vUSER_NAME", condition.USER_NAME));
            cmd.Parameters.Add(new MySqlParameter("?vUSER_SURNAME", condition.USER_SURNAME));
            cmd.Parameters.Add(new MySqlParameter("?vEMAIL", condition.EMAIL));
            cmd.Parameters.Add(new MySqlParameter("?vPASSWORD", condition.PASSWORD));
            cmd.Parameters.Add(new MySqlParameter("?vROLE_ID", condition.ROLE_ID));
            cmd.ExecuteNonQuery();
            cmd.Dispose();

        }

        public void ADD_LOGO(DataFile condition, int? tour_id)
        {
            MySqlCommand cmd = new MySqlCommand();
            List<TT_MENU> result = new List<TT_MENU>();
            cmd.Connection = connection;
            cmd.CommandText = @"INSERT INTO `table_tennis`.`tt_image_file`
                                        ( `tour_id`,
                                         `header`,
                                         `name`,
                                         `type`,
                                         `size`,
                                         `path_file`,
                                          uri_path)
                            VALUES ( ?vtour_id,
                                    ?vheader,
                                    ?vname,
                                    ?vtype,
                                    ?vsize,
                                    ?vpath_file,
                                    ?vuri_path) ";
            cmd.Parameters.Add(new MySqlParameter("?vtour_id", tour_id));
            cmd.Parameters.Add(new MySqlParameter("?vheader", condition.header));
            cmd.Parameters.Add(new MySqlParameter("?vname", condition.name));
            cmd.Parameters.Add(new MySqlParameter("?vtype", condition.type));
            cmd.Parameters.Add(new MySqlParameter("?vsize", condition.size));
            cmd.Parameters.Add(new MySqlParameter("?vpath_file", condition.path_file));
            cmd.Parameters.Add(new MySqlParameter("?vuri_path", condition.uri_path));

            cmd.ExecuteNonQuery();
            cmd.Dispose();

        }
        public void ADD_Ranking(RANKING_DETAIL[] dataObject)
        {
            StringBuilder sCommand = new StringBuilder(@"INSERT INTO `table_tennis`.`rank_detail`
            (
             `RANKING_ID`,
             `RANK_CODE`,
             `PLAYER_ID`,
             `POINT`,
             `BELONG_TO`,
             `RANK`)
            VALUES ");
            int? count = dataObject.Length;
            List<string> Rows = new List<string>();
            for (int i = 0; i < count; i++)
            {
                Rows.Add(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}')",
                    MySqlHelper.EscapeString(dataObject[i].RANKING_ID + ""),
                    MySqlHelper.EscapeString(dataObject[i].RANK_CODE + ""),
                    MySqlHelper.EscapeString(dataObject[i].PLAYER_ID + ""),
                    MySqlHelper.EscapeString(dataObject[i].POINT + ""),
                    MySqlHelper.EscapeString(dataObject[i].BELONG_TO + ""),
                    MySqlHelper.EscapeString(dataObject[i].RANK + "")
                    ));
            }
            sCommand.Append(string.Join(",", Rows));
            sCommand.Append(";");
            using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString()))
            {
                myCmd.CommandType = CommandType.Text;
                myCmd.Connection = connection;
                myCmd.ExecuteNonQuery();
            }


        }

        public void ADD_Ranking_id(out long? rankingID, RANKING dataObject)
        {
            rankingID = null;
            MySqlCommand cmd = new MySqlCommand();
            List<TT_MENU> result = new List<TT_MENU>();
            cmd.Connection = connection;
            cmd.CommandText = @"INSERT INTO `table_tennis`.`ranking`
            (
             `DESC`,
             `CREATE_ON`,
             `CREATE_BY`,          
             `TERNINATE_FLG`)
VALUES (
        ?vDESC,
        SYSDATE() ,
        ?vCREATE_BY,        
        'N') ";
            cmd.Parameters.Add(new MySqlParameter("?vDESC", dataObject.DESC.Trim()));
            cmd.Parameters.Add(new MySqlParameter("?vCREATE_BY", dataObject.CREATE_BY.Trim()));
            //rankingID = cmd.LastInsertedId;
            cmd.ExecuteNonQuery();
            cmd.CommandText = "SELECT LAST_INSERT_ID()";
            // cmd.CommandText = "SELECT LAST_INSERT_ID()";
            rankingID = Convert.ToInt64(cmd.ExecuteScalar());
            cmd.Dispose();
        }

        #endregion

        #region " ดึงข้อมูล "

        #region " มาสเตอร์ดาต้า "
        public void GET_RANK_DT(out List<RANKING_DETAIL> dataObjects, long? tourID)
        {
            MySqlCommand cmd = new MySqlCommand();
            TT_USER result = new TT_USER();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT
                         r.RANK_CODE
                        FROM `table_tennis`.`rank_detail` r
                        INNER JOIN 
                        `table_tennis`.`tournament` t
			            ON r.`RANKING_ID` =t.`RANKING_ID`
                        WHERE t.`ID` =?vTOUR_ID
                        GROUP BY r.RANK_CODE";
            cmd.Parameters.Add(new MySqlParameter("?vTOUR_ID", tourID));
            cmd.ExecuteNonQuery();
            using (DataTable dt = Utility.FillDataTable(cmd))
            {
                dataObjects = dt.AsEnumerable<RANKING_DETAIL>().ToList();
            }
            cmd.Dispose();
        }
        public void GET_RANK_DT_CODE(out List<RANKING_DETAIL> dataObjects, long? rankId)
        {
            MySqlCommand cmd = new MySqlCommand();
            TT_USER result = new TT_USER();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT
                         r.RANK_CODE
                        FROM `table_tennis`.`rank_detail` r                      
                        WHERE r.`RANKING_ID` =?vRANKING_ID
                        GROUP BY r.RANK_CODE";
            cmd.Parameters.Add(new MySqlParameter("?vRANKING_ID", rankId));
            cmd.ExecuteNonQuery();
            using (DataTable dt = Utility.FillDataTable(cmd))
            {
                dataObjects = dt.AsEnumerable<RANKING_DETAIL>().ToList();
            }
            cmd.Dispose();
        }
        public void GET_RANK_DETAIL(out List<RANKING_DETAIL> dataObjects, long? rankId, string rankcode)
        {
            MySqlCommand cmd = new MySqlCommand();
            TT_USER result = new TT_USER();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT
                              dt.*,concat( pl.FIRST_NAME ,' ', pl.LAST_NAME) as NAME
                            FROM `table_tennis`.`rank_detail` dt
                            INNER JOIN `table_tennis`.`player` pl
                            ON pl.ID =  dt.`PLAYER_ID`
                            where dt.RANKING_ID =?vRANKING_ID and dt.RANK_CODE =?vRANK_CODE AND  pl.TENINATE_FLG ='N' ";
            cmd.Parameters.Add(new MySqlParameter("?vRANKING_ID", rankId));
            cmd.Parameters.Add(new MySqlParameter("?vRANK_CODE", rankcode));
            cmd.ExecuteNonQuery();
            using (DataTable dt = Utility.FillDataTable(cmd))
            {
                dataObjects = dt.AsEnumerable<RANKING_DETAIL>().ToList();
            }
            cmd.Dispose();
        }
        public List<TOURNAMENT> GET_TOUR_CODE()
        {
            StringBuilder sqlSb = new StringBuilder();
            sqlSb.AppendLine(@" SELECT t.* FROM tournament  t                             
                               where t.TENINATE_FLG='N' ORDER BY ID DESC
                                ");
            return Utility.ExecuteToList<TOURNAMENT>(sqlSb.ToString(), connection);
        }
        public List<CATEGORY> GET_CATEGORY()
        {
            StringBuilder sqlSb = new StringBuilder();
            sqlSb.AppendLine("SELECT * FROM  TABLE_TENNIS.CATEGORY where TMN_FLAG='N'");
            return Utility.ExecuteToList<CATEGORY>(sqlSb.ToString(), connection);
        }
        public List<EVENT> GetEvent()
        {
            StringBuilder sqlSb = new StringBuilder();

            sqlSb.AppendLine(@"SELECT Ev.* FROM `event` EV WHERE EV.`TENINATE_FLG` ='N'");
            return Utility.ExecuteToList<EVENT>(sqlSb.ToString(), connection);
        }

        public List<GENDER> GetGender()
        {
            StringBuilder sqlSb = new StringBuilder();

            sqlSb.AppendLine(@"SELECT * FROM `GENDER` GN WHERE GN.`TMN_FLG` ='N'");
            return (Utility.ExecuteToList<GENDER>(sqlSb.ToString(), connection));
        }

        public List<TT_ROLE> GET_ROLE()
        {
            StringBuilder sqlSb = new StringBuilder();

            sqlSb.AppendLine(@"SELECT * FROM `tt_role` WHERE TERMINATE_DATE IS NULL ");
            return (Utility.ExecuteToList<TT_ROLE>(sqlSb.ToString(), connection));
        }

        public List<RANKING> GetRanking()
        {
            StringBuilder sqlSb = new StringBuilder();

            sqlSb.AppendLine(@"SELECT * FROM table_tennis.ranking GN WHERE GN.`TERNINATE_FLG` ='N'");
            return (Utility.ExecuteToList<RANKING>(sqlSb.ToString(), connection));
        }
        #endregion

        #region " Login "
        public TT_USER ValidateUserEmail(string email, string Password)
        {
            MySqlCommand cmd = new MySqlCommand();
            TT_USER result = new TT_USER();
            cmd.Connection = connection;
            cmd.CommandText = "SELECT * FROM  tt_user WHERE EMAIL = ?IN_EMAIL AND PASSWORD = ?IN_PASSWORD AND TMN_DT IS NULL";
            cmd.Parameters.Add(new MySqlParameter("?IN_EMAIL", email));
            cmd.Parameters.Add(new MySqlParameter("?IN_PASSWORD", Password));
            cmd.ExecuteNonQuery();

            using (DataTable dt = Utility.FillDataTable(cmd))
            {
                result = dt.AsEnumerable<TT_USER>().FirstOrDefault();
            }
            cmd.Dispose();
            return result;
        }

        public TT_USER ValidateUserID(string user_id, string Password)
        {
            MySqlCommand cmd = new MySqlCommand();
            TT_USER result = new TT_USER();
            cmd.Connection = connection;
            cmd.CommandText = "SELECT * FROM  tt_user WHERE USER_ID =  ?IN_USER_ID AND PASSWORD = ?IN_PASSWORD AND TMN_DT IS NULL";
            cmd.Parameters.Add(new MySqlParameter("?IN_USER_ID", user_id));
            cmd.Parameters.Add(new MySqlParameter("?IN_PASSWORD", Password));
            cmd.ExecuteNonQuery();

            using (DataTable dt = Utility.FillDataTable(cmd))
            {
                result = dt.AsEnumerable<TT_USER>().FirstOrDefault();
            }
            cmd.Dispose();
            return result;
        }

        public List<TT_MENU> GET_MENU(int? role_id)
        {
            MySqlCommand cmd = new MySqlCommand();
            List<TT_MENU> result = new List<TT_MENU>();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT TM.* FROM tt_MENU_CONTROL TMC INNER JOIN TT_MENU TM
                                ON TMC.MENU_ID = TM.`MENU_ID`
                                WHERE TMC.`ROLE_ID` = ?IN_ROLE_ID AND TM.TERMINATE_DT IS NULL ORDER BY MENU_SEQ ASC";
            cmd.Parameters.Add(new MySqlParameter("?IN_ROLE_ID", role_id));
            cmd.ExecuteNonQuery();
            using (DataTable dt = Utility.FillDataTable(cmd))
            {
                result = dt.AsEnumerable<TT_MENU>().ToList();
            }
            cmd.Dispose();
            return result;
        }

        public List<TT_ACCESS_TYPE> GET_ACCESS_TYPE(int? role_id)
        {
            MySqlCommand cmd = new MySqlCommand();
            List<TT_ACCESS_TYPE> result = new List<TT_ACCESS_TYPE>();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT TMC.* FROM `tt_access_type` TMC INNER JOIN `tt_user_control` TM
                                ON TMC.`ID` = TM.`ACCESS_TYPE_ID`
                                WHERE TM.`ROLE_ID` = ?IN_ROLE_ID AND TM.TMN_DT IS NULL ";
            cmd.Parameters.Add(new MySqlParameter("?IN_ROLE_ID", role_id));
            cmd.ExecuteNonQuery();
            using (DataTable dt = Utility.FillDataTable(cmd))
            {
                result = dt.AsEnumerable<TT_ACCESS_TYPE>().ToList();
            }
            cmd.Dispose();
            return result;

        }

        #endregion

        #region " ดึงข้อมูล user "
        public void GET_USER_SEARCH(out List<DATAUSER> dataObjects, TT_USER condition)
        {
            MySqlCommand cmd = new MySqlCommand();
            TT_USER result = new TT_USER();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT * FROM  tt_user tu
                                INNER JOIN 
                                tt_ROLE tr
                                ON
                                tr.ROLE_ID= tu.ROLE_ID
                                WHERE tr.`TERMINATE_DATE` IS NULL
                                AND tu.`TMN_DT` IS NULL ";

            if (condition.ROLE_ID != null)
            {
                cmd.CommandText = cmd.CommandText + @" AND tr.ROLE_ID = ?IN_ROLE_ID ";
                cmd.Parameters.Add(new MySqlParameter("?IN_ROLE_ID", condition.ROLE_ID));

            }
            if (!string.IsNullOrEmpty(condition.USER_NAME))
            {
                cmd.CommandText = cmd.CommandText + @" AND tu.USER_NAME LIKE ?IN_USER_NAME";
                //cmd.Parameters.Add(new MySqlParameter("?IN_USER_NAME", condition.USER_NAME));
                cmd.Parameters.AddWithValue("?IN_USER_NAME", condition.USER_NAME + "%");
            }
            if (!string.IsNullOrEmpty(condition.USER_SURNAME))
            {
                cmd.CommandText = cmd.CommandText + @" AND tu.USER_SURNAME LIKE ?IN_USER_SURNAME";
                //cmd.Parameters.Add(new MySqlParameter("?IN_USER_SURNAME", condition.USER_SURNAME));
                cmd.Parameters.AddWithValue("?IN_USER_SURNAME", condition.USER_SURNAME + "%");
            }
            if (!string.IsNullOrEmpty(condition.EMAIL))
            {
                cmd.CommandText = cmd.CommandText + @" AND tu.EMAIL LIKE ?IN_EMAIL";
                //cmd.Parameters.Add(new MySqlParameter("?IN_EMAIL", condition.EMAIL));
                cmd.Parameters.AddWithValue("?IN_EMAIL", condition.EMAIL + "%");

            }
            if (!string.IsNullOrEmpty(condition.USER_ID))
            {
                cmd.CommandText = cmd.CommandText + @" AND tu.USER_ID = ?IN_USER_ID";
                //cmd.Parameters.Add(new MySqlParameter("?IN_USER_ID", condition.USER_ID));
                cmd.Parameters.AddWithValue("?IN_USER_ID", condition.USER_ID);
            }
            cmd.CommandText = cmd.CommandText + " LIMIT 0, 1000 ";
            cmd.ExecuteNonQuery();

            using (DataTable dt = Utility.FillDataTable(cmd))
            {
                dataObjects = dt.AsEnumerable<DATAUSER>().ToList();
            }
            cmd.Dispose();

        }

        public void GET_USER_SEARCH_MANGER(out List<DATAUSER> dataObjects, TT_USER condition)
        {
            MySqlCommand cmd = new MySqlCommand();
            TT_USER result = new TT_USER();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT  ID,                           
                              `MANAGER_CODE` as USER_ID,
                              `FIRST_NAME` AS USER_NAME,
                              `LAST_NAME` AS USER_SURNAME,                             
                              `EMAIL`,
                               (SELECT  ROLE_DESCRIPTION FROM tt_role WHERE ROLE_ID = ?IN_ROLE_ID) AS  ROLE_DESCRIPTION ,
                                PHONE,                                
                                GENDER  ,
                                TEAM                                                                            
                            FROM `table_tennis`.`manager` WHERE TEMINATE_FLG ='N' ";
            cmd.Parameters.Add(new MySqlParameter("?IN_ROLE_ID", condition.ROLE_ID));

            if (!string.IsNullOrEmpty(condition.USER_NAME))
            {
                cmd.CommandText = cmd.CommandText + @" AND FIRST_NAME LIKE ?IN_FIRST_NAME";
                //cmd.Parameters.Add(new MySqlParameter("?IN_USER_NAME", condition.USER_NAME));
                cmd.Parameters.AddWithValue("?IN_FIRST_NAME", condition.USER_NAME + "%");
            }
            if (!string.IsNullOrEmpty(condition.USER_SURNAME))
            {
                cmd.CommandText = cmd.CommandText + @" AND LAST_NAME LIKE ?IN_LAST_NAME";
                //cmd.Parameters.Add(new MySqlParameter("?IN_USER_SURNAME", condition.USER_SURNAME));
                cmd.Parameters.AddWithValue("?IN_LAST_NAME", condition.USER_SURNAME + "%");
            }
            if (!string.IsNullOrEmpty(condition.EMAIL))
            {
                cmd.CommandText = cmd.CommandText + @" AND EMAIL LIKE ?IN_EMAIL";
                //cmd.Parameters.Add(new MySqlParameter("?IN_EMAIL", condition.EMAIL));
                cmd.Parameters.AddWithValue("?IN_EMAIL", condition.EMAIL + "%");

            }
            if (!string.IsNullOrEmpty(condition.USER_ID))
            {
                cmd.CommandText = cmd.CommandText + @" AND ID = ?IN_ID";
                //cmd.Parameters.Add(new MySqlParameter("?IN_USER_ID", condition.USER_ID));
                cmd.Parameters.AddWithValue("?IN_ID", condition.USER_ID);
            }
            cmd.CommandText = cmd.CommandText + " LIMIT 0, 1000 ";
            cmd.ExecuteNonQuery();

            using (DataTable dt = Utility.FillDataTable(cmd))
            {
                dataObjects = dt.AsEnumerable<DATAUSER>().ToList();
            }
            cmd.Dispose();

        }

        public void GET_USER_SEARCH_PLAYER(out List<DATAUSER> dataObjects, TT_USER condition)
        {
            MySqlCommand cmd = new MySqlCommand();
            TT_USER result = new TT_USER();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT   ID,                         
                              `PLAYER_CODE` as USER_ID,
                              `FIRST_NAME` AS USER_NAME,
                              `LAST_NAME` AS USER_SURNAME,                             
                              `EMAIL`,
                                PHONE,
                                BIRTH_DATE,
                                GENDER ,                              
                               (SELECT  ROLE_DESCRIPTION FROM tt_role WHERE ROLE_ID = ?IN_ROLE_ID) AS  ROLE_DESCRIPTION                                                                               
                            FROM `table_tennis`.`player` WHERE TENINATE_FLG ='N' ";

            cmd.Parameters.Add(new MySqlParameter("?IN_ROLE_ID", condition.ROLE_ID));

            if (!string.IsNullOrEmpty(condition.USER_NAME))
            {
                cmd.CommandText = cmd.CommandText + @" AND FIRST_NAME LIKE ?IN_FIRST_NAME";
                //cmd.Parameters.Add(new MySqlParameter("?IN_USER_NAME", condition.USER_NAME));
                cmd.Parameters.AddWithValue("?IN_FIRST_NAME", condition.USER_NAME + "%");
            }
            if (!string.IsNullOrEmpty(condition.USER_SURNAME))
            {
                cmd.CommandText = cmd.CommandText + @" AND LAST_NAME LIKE ?IN_LAST_NAME";
                //cmd.Parameters.Add(new MySqlParameter("?IN_USER_SURNAME", condition.USER_SURNAME));
                cmd.Parameters.AddWithValue("?IN_LAST_NAME", condition.USER_SURNAME + "%");
            }
            if (!string.IsNullOrEmpty(condition.EMAIL))
            {
                cmd.CommandText = cmd.CommandText + @" AND EMAIL LIKE ?IN_EMAIL";
                //cmd.Parameters.Add(new MySqlParameter("?IN_EMAIL", condition.EMAIL));
                cmd.Parameters.AddWithValue("?IN_EMAIL", condition.EMAIL + "%");

            }
            if (!string.IsNullOrEmpty(condition.USER_ID))
            {
                cmd.CommandText = cmd.CommandText + @" AND ID = ?IN_ID";
                //cmd.Parameters.Add(new MySqlParameter("?IN_USER_ID", condition.USER_ID));
                cmd.Parameters.AddWithValue("?IN_ID", condition.USER_ID);
            }
            cmd.CommandText = cmd.CommandText + " LIMIT 0, 1000 ";
            cmd.ExecuteNonQuery();

            using (DataTable dt = Utility.FillDataTable(cmd))
            {
                dataObjects = dt.AsEnumerable<DATAUSER>().ToList();
            }
            cmd.Dispose();

        }
        #endregion

        #region " FILE "
        public void GET_LOGO(out DataFile[] result, int? tour_id)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT * FROM `table_tennis`.`tt_image_file` WHERE TOUR_ID =?vtour_id AND TMN_DT IS NULL";
            cmd.Parameters.Add(new MySqlParameter("?vtour_id", tour_id));
            cmd.ExecuteNonQuery();

            using (DataTable dt = Utility.FillDataTable(cmd))
            {
                result = dt.AsEnumerable<DataFile>().ToArray();
            }
            cmd.Dispose();
        }
        #endregion

        public List<TOUR_MAPBASE> GetEventInTourMapbase(TOURNAMENT dataObject, int? cate)
        {
            StringBuilder sqlSb = new StringBuilder();
            List<MySqlParam> param = new List<MySqlParam>();
            sqlSb.AppendLine(@"SELECT TM.* FROM TOURNAMENT T
                                    INNER JOIN 
                                    TOUR_MAPBASE TM
                                    ON 
                                    T.ID =TM.TOURNAMENT_ID
                                    WHERE T.ID =@TOUR_ID AND TM.CAT_ID = @CAT_ID AND TM.TENINATE_FLG ='N'");
            param.Add(new MySqlParam("@TOUR_ID", dataObject.ID, MySqlDbType.Int64));
            param.Add(new MySqlParam("@CAT_ID", cate, MySqlDbType.Int64));
            return Utility.ExecuteToList<TOUR_MAPBASE>(sqlSb.ToString(), connection, param);
        }

        public void Get_CATE_MAP_TOUR(out CATEGORY dataObjects, TOURNAMENT condition, int? concate)
        {
            MySqlCommand cmd = new MySqlCommand();
            TT_USER result = new TT_USER();
            cmd.Connection = connection;
            cmd.CommandText = @"SELECT
                               ct.ID,
                               ct.DESCRIPTION,
                               ct.NAME_CODE,
                               IFNULL(cmt.NUM_OF_PLY,ct.NUM_OF_PLY) AS NUM_OF_PLY
                             FROM table_tennis.cat_map_tour cmt 
                             INNER JOIN 
                             category ct ON cmt.CAT_ID = ct.ID
                             WHERE cmt.TOUR_ID =?IN_TOUR_ID AND cmt.CAT_ID =?vCAT_ID ";

            cmd.Parameters.Add(new MySqlParameter("?IN_TOUR_ID", condition.ID));
            cmd.Parameters.Add(new MySqlParameter("?vCAT_ID", concate));

            using (DataTable dt = Utility.FillDataTable(cmd))
            {
                dataObjects = dt.AsEnumerable<CATEGORY>().FirstOrDefault();
            }
            cmd.Dispose();

        }

        #endregion

        #region " Stored Procedure "
        public void AddEvent(int? TOUR_ID, EVENT dataObj, int? category, int? num_of_ply)
        {
            string flag = "N";
            MySqlCommand cmd = new MySqlCommand(@"ADD_EVENT", connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IN_TOUR_ID", TOUR_ID);
            cmd.Parameters["@IN_TOUR_ID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_EVENT_ID", dataObj.ID);
            cmd.Parameters["@IN_EVENT_ID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_EVENT_CODE", dataObj.CODE);
            cmd.Parameters["@IN_EVENT_CODE"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_EN_EVENT_NAME", dataObj.EN_EVENT_NAME);
            cmd.Parameters["@IN_EN_EVENT_NAME"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_TH_EVENT_NAME", dataObj.TH_EVENT_NAME);
            cmd.Parameters["@IN_TH_EVENT_NAME"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_DESCRIPTION", dataObj.DESCRIPTION);
            cmd.Parameters["@IN_DESCRIPTION"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_CREATE_BY", dataObj.CREATE_BY);
            cmd.Parameters["@IN_CREATE_BY"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_MODIFY_BY", dataObj.MODIFY_BY);
            cmd.Parameters["@IN_MODIFY_BY"].Direction = ParameterDirection.Input;
            if (dataObj.TOUR_SELECT)
            {
                flag = "Y";
            }
            cmd.Parameters.AddWithValue("@IN_CHOOSE", flag);
            cmd.Parameters["@IN_CHOOSE"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_GEN_CODE", dataObj.GEN_CODE ?? "N");
            cmd.Parameters["@IN_GEN_CODE"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_PRICE", dataObj.PRICE ?? 0);
            cmd.Parameters["@IN_PRICE"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_CATE_ID", category);
            cmd.Parameters["@IN_CATE_ID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_NUM_OF_PLY", num_of_ply);
            cmd.Parameters["@IN_NUM_OF_PLY"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_RANK_CODE", dataObj.RANK_CODE);
            cmd.Parameters["@IN_RANK_CODE"].Direction = ParameterDirection.Input;

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public void RemoveUserAdmin(TT_USER dataObject, string userId)
        {
            MySqlCommand cmd = new MySqlCommand(@"REMOVE_USER", connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IN_ID", dataObject.ID);
            cmd.Parameters["@IN_ID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_ROLE_ID", dataObject.ROLE_ID);
            cmd.Parameters["@IN_ROLE_ID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_USER_MODIFY", userId);
            cmd.Parameters["@IN_USER_MODIFY"].Direction = ParameterDirection.Input;

            cmd.ExecuteNonQuery();
            cmd.Dispose();

        }

        public void UpdateUser(DATAUSER dataObject, string userlogin)
        {
            MySqlCommand cmd = new MySqlCommand(@"UPDATE_USER", connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IN_ID", dataObject.ID);
            cmd.Parameters["@IN_ID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_USER_UPDATE", userlogin);
            cmd.Parameters["@IN_USER_UPDATE"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_USER_ID", dataObject.USER_ID);
            cmd.Parameters["@IN_USER_ID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_USER_NAME", dataObject.USER_NAME);
            cmd.Parameters["@IN_USER_NAME"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_USER_SURNAME", dataObject.USER_SURNAME);
            cmd.Parameters["@IN_USER_SURNAME"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_EMAIL", dataObject.EMAIL);
            cmd.Parameters["@IN_EMAIL"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_ROLE_ID", dataObject.ROLE_ID);
            cmd.Parameters["@IN_ROLE_ID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_PHONE", dataObject.PHONE);
            cmd.Parameters["@IN_PHONE"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_BIRTH_DATE", dataObject.BIRTH_DATE);
            cmd.Parameters["@IN_BIRTH_DATE"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_GENDER", dataObject.GENDER);
            cmd.Parameters["@IN_GENDER"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@IN_TEAM", dataObject.TEAM);
            cmd.Parameters["@IN_TEAM"].Direction = ParameterDirection.Input;

            cmd.ExecuteNonQuery();
            cmd.Dispose();

        }
        #endregion

        #region " ลบข้อมูล "
        public void RemoveEvent(EVENT dataObject, string userId)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"UPDATE  EVENT
                               SET
                                TENINATE_FLG ='Y',
                                MODIFY_ON = CURDATE(),
                                MODIFY_BY = ?vUserId
                                WHERE ID =?vID ";
            cmd.Parameters.Add(new MySqlParameter("?vID", dataObject.ID));
            cmd.Parameters.Add(new MySqlParameter("?vUserId", userId));
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public void RemoveTour(TOURNAMENT dataObject, string userId)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"UPDATE  TOURNAMENT
                               SET
                               TENINATE_FLG = 'Y',
                               MODIFY_ON = CURDATE(),
                               MODIFY_BY = ?vUserId
                              WHERE ID =?vID ";
            cmd.Parameters.Add(new MySqlParameter("?vID", dataObject.ID));
            cmd.Parameters.Add(new MySqlParameter("?vUserId", userId));
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public void RemoveFile(DataFile dataObject, string userId)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"UPDATE  `table_tennis`.`tt_image_file`
                                SET TMN_DT = CURDATE(),
                                TMN_ID =?vUserID
                                WHERE image_id = ?vimage_id ";
            cmd.Parameters.Add(new MySqlParameter("?vimage_id", dataObject.image_id));
            cmd.Parameters.Add(new MySqlParameter("?vUserID", userId));

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
        public void RemoveRank(RANKING dataObject)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"DELETE
FROM `table_tennis`.`rank_detail`
WHERE `RANKING_ID` = ?vRANKING_ID";
            cmd.Parameters.Add(new MySqlParameter("?vRANKING_ID", dataObject.ID));


            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }


        #endregion

        #region " อัพเดท "    
        public void UpdateRank_modify(RANKING dataObject, string userID)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"UPDATE `table_tennis`.`ranking`
                                SET                                                                 
                                  `MODIFY_ON` = CURDATE(),
                                  `MODIFY_BY` = ?vMODIFY_BY                                 
                                WHERE `ID` = ?vID";
            cmd.Parameters.Add(new MySqlParameter("?vMODIFY_BY", userID));
            cmd.Parameters.Add(new MySqlParameter("?vID", dataObject.ID));
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
        public void UpdateTournament(_TOURNAMENT dataObject)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"UPDATE table_tennis.tournament
                                SET 
                                 EN_TOUR_NAME = ?vEN_TOUR_NAME,
                                 TH_TOUR_NAME = ?vTH_TOUR_NAME,
                                 DESCRIPTION = ?vDESCRIPTION,
                                 TOUR_POINT = ?vTOUR_POINT,
                                 TOUR_ADDRESS = ?vTOUR_ADDRESS,                                                      
                                 MODIFY_ON =  SYSDATE(),
                                 MODIFY_BY = ?vMODIFY_BY,
                                 EXPIRE_ON = ?vEXPIRE_ON,
                                 TENINATE_FLG = ?vTENINATE_FLG,
                                 TOUR_START_ON = ?vTOUR_START_ON,
                                 TOUR_END_ON = ?vTOUR_END_ON,
                                 RANKING_ID =?vRANKING_ID
                                WHERE ID = ?vID";
            cmd.Parameters.Add(new MySqlParameter("?vEN_TOUR_NAME", dataObject.EN_TOUR_NAME));
            cmd.Parameters.Add(new MySqlParameter("?vTH_TOUR_NAME", dataObject.TH_TOUR_NAME));
            cmd.Parameters.Add(new MySqlParameter("?vDESCRIPTION", dataObject.DESCRIPTION));
            cmd.Parameters.Add(new MySqlParameter("?vTOUR_POINT", dataObject.TOUR_POINT));
            cmd.Parameters.Add(new MySqlParameter("?vTOUR_ADDRESS", dataObject.TOUR_ADDRESS));
            cmd.Parameters.Add(new MySqlParameter("?vMODIFY_BY", dataObject.MODIFY_BY));
            cmd.Parameters.Add(new MySqlParameter("?vEXPIRE_ON", dataObject.EXPIRE_ON));
            cmd.Parameters.Add(new MySqlParameter("?vTENINATE_FLG", dataObject.TENINATE_FLG));
            cmd.Parameters.Add(new MySqlParameter("?vTOUR_START_ON", dataObject.TOUR_START_ON));
            cmd.Parameters.Add(new MySqlParameter("?vTOUR_END_ON", dataObject.TOUR_END_ON));
            cmd.Parameters.Add(new MySqlParameter("?vRANKING_ID", dataObject.RANKING_ID));
            cmd.Parameters.Add(new MySqlParameter("?vID", dataObject.ID));

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        #endregion
    }
}
