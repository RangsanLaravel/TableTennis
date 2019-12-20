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
    public class PlayerAccess : RepositoryBase
    {
        public PlayerAccess(string connectionName)
        {
            base.Repository(connectionName);
        }

        public void Add(ref PLAYER dataObj)
        {
            MySqlCommand cmd = new MySqlCommand(@"ADD_PLAYER", connection);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@vSALUTATION", dataObj.SALUTATION);
            cmd.Parameters["@vSALUTATION"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@vFIRST_NAME", dataObj.FIRST_NAME);
            cmd.Parameters["@vFIRST_NAME"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@vLAST_NAME", dataObj.LAST_NAME);
            cmd.Parameters["@vLAST_NAME"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@vGENDER", dataObj.GENDER);
            cmd.Parameters["@vGENDER"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@vEMAIL", dataObj.EMAIL);
            cmd.Parameters["@vEMAIL"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@vPHONE", dataObj.PHONE);
            cmd.Parameters["@vPHONE"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@vBIRTH_DATE",Utility.stringTodate(dataObj.BIRTH_DATE_STR));
            cmd.Parameters["@vBIRTH_DATE"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@vPLAYER_CODE", MySqlDbType.VarChar);
            cmd.Parameters["@vPLAYER_CODE"].Direction = ParameterDirection.Output;
            cmd.Parameters.AddWithValue("@vID", MySqlDbType.Int64);
            cmd.Parameters["@vID"].Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            dataObj.ID = (int)cmd.Parameters["@vID"].Value;
            dataObj.PLAYER_CODE = cmd.Parameters["@vPLAYER_CODE"].Value.ToString();
        }

        public List<PLAYER> GridResult(ParamPlayer param)
        {
            PLAYER dataObj = new PLAYER();
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
                _param.Add(new MySqlParam("vNAME", "%" + param.NAME + "%", MySqlDbType.VarChar));
            }

            if (param.GENDER!=null)
            {
                condition.AppendLine(@"and GENDER = @vGENDER");
                _param.Add(new MySqlParam("vGENDER", param.GENDER, MySqlDbType.VarChar));
            }
            sqlSb.AppendLine("SELECT * FROM player WHERE 1=1 ");
            sqlSb.AppendLine(condition.ToString());
            //sqlSb.AppendLine(@"limit 1000");
            return Utility.ExecuteToList<PLAYER>(sqlSb.ToString(), connection, _param);
        }

        public PLAYER Get(int? id)
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
    }
}
