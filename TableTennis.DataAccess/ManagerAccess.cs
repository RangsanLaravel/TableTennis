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
    public class ManagerAccess:RepositoryBase
    {
        public ManagerAccess(string connectionName)
        {
            base.Repository(connectionName);
        }

        public void Add(ref MANAGER dataObj)
        {           
            MySqlCommand cmd = new MySqlCommand(@"ADD_MANAGER",connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@VFIRST_NAME", dataObj.FIRST_NAME);
            cmd.Parameters["@VFIRST_NAME"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@VLAST_NAME", dataObj.LAST_NAME);
            cmd.Parameters["@VLAST_NAME"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@VGENDER", dataObj.GENDER);
            cmd.Parameters["@VGENDER"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@VEMAIL", dataObj.EMAIL);
            cmd.Parameters["@VEMAIL"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@VPHONE", dataObj.PHONE);
            cmd.Parameters["@VPHONE"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@VTEAM", dataObj.TEAM);
            cmd.Parameters["@VTEAM"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@VMANAGER_CODE",MySqlDbType.VarChar);
            cmd.Parameters["@VMANAGER_CODE"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@VID", MySqlDbType.Int64);
            cmd.Parameters["@VID"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            dataObj.ID = (int)cmd.Parameters["@VID"].Value;
            dataObj.MANAGER_CODE = cmd.Parameters["@VMANAGER_CODE"].Value.ToString();
        }

        public MANAGER SignIn(string email, int? id)
        {
            MANAGER dataObj = new MANAGER();
            StringBuilder sqlSb = new StringBuilder();
            List<MySqlParam> param = new List<MySqlParam>();
            param.Add(new MySqlParam("EMAIL", email, MySqlDbType.VarChar));
            param.Add(new MySqlParam("ID", id, MySqlDbType.Int32));
            sqlSb.AppendLine("SELECT * FROM manager WHERE EMAIL =@EMAIL AND ID = @ID");
            return Utility.ExecuteToList<MANAGER>(sqlSb.ToString(), connection, param).FirstOrDefault();
        }

        public List<MANAGER> GridManager(ParamManager param)
        {
            StringBuilder sqlSb = new StringBuilder();
            StringBuilder condition = new StringBuilder();
            List<MySqlParam> _param = new List<MySqlParam>();
            if (param.CODE != null)
            {
                condition.AppendLine(@" and ID =@vID");
                _param.Add(new MySqlParam("vID", param.CODE, MySqlDbType.Int64));
            }

            if (!string.IsNullOrEmpty(param.NAME))
            {
                condition.AppendLine(@"and CONCAT(FIRST_NAME,' ',LAST_NAME) LIKE @vNAME");
                _param.Add(new MySqlParam("vNAME","%"+ param.NAME + "%",MySqlDbType.VarChar));
            }

            if (!string.IsNullOrEmpty(param.TEAM))
            {
                condition.AppendLine(@"and TEAM like @vTEAM");
                _param.Add(new MySqlParam("vTEAM", "%" + param.TEAM + "%", MySqlDbType.VarChar));
            }

            sqlSb.AppendLine("SELECT * FROM manager WHERE 1=1");
            sqlSb.AppendLine(condition.ToString());
            return Utility.ExecuteToList<MANAGER>(sqlSb.ToString(), connection, _param);
        }

        public MANAGER SendRecover(string email)
        {
            MANAGER dataObj = new MANAGER();
            StringBuilder sqlSb = new StringBuilder();
            List<MySqlParam> param = new List<MySqlParam>();
            param.Add(new MySqlParam("EMAIL", email, MySqlDbType.VarChar));
            sqlSb.AppendLine("SELECT * FROM manager WHERE EMAIL = @EMAIL");
            return Utility.ExecuteToList<MANAGER>(sqlSb.ToString(), connection, param).FirstOrDefault();
        }

        public void Update(ref MANAGER dataObj)
        {
            MySqlCommand cmd = new MySqlCommand(@"UPDATE_MANAGER", connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@VFIRST_NAME", dataObj.FIRST_NAME);
            cmd.Parameters["@VFIRST_NAME"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@VLAST_NAME", dataObj.LAST_NAME);
            cmd.Parameters["@VLAST_NAME"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@VGENDER", dataObj.GENDER);
            cmd.Parameters["@VGENDER"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@VPHONE", dataObj.PHONE);
            cmd.Parameters["@VPHONE"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@VTEAM", dataObj.TEAM);
            cmd.Parameters["@VTEAM"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@VID", dataObj.ID);
            cmd.Parameters["@VID"].Direction = ParameterDirection.Input;

            cmd.ExecuteNonQuery();
        }

        public MANAGER Get(long? id)
        {
            if (id == null)
                return null;

            MANAGER dataObj = new MANAGER();
            StringBuilder sqlSb = new StringBuilder();
            List<MySqlParam> param = new List<MySqlParam>();
            param.Add(new MySqlParam("ID", id, MySqlDbType.Int64));
            sqlSb.AppendLine("SELECT * FROM manager WHERE ID =@ID");
            return Utility.ExecuteToList<MANAGER>(sqlSb.ToString(), connection,param).FirstOrDefault();
        }
    }
}
