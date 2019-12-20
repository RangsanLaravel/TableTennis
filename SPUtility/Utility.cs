using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SPUtility
{
    public static class Utility
    {
        
        public static List<T> ExecuteToList<T>(string sqlStr, MySqlConnection connection,List<MySqlParam> param = null) where T : new()
        {
            List<T> list = new List<T>();
            MySqlCommand cmd = new MySqlCommand(sqlStr, connection);
            if (param != null)            
                foreach (var item in param)
                {
                    //Add parameter
                    cmd.Parameters.Add(item.ParamName,item.DbType).Value = item.Value;
                }
            

            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                T entity = new T();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    PropertyInfo property = typeof(T).GetProperty(reader.GetName(i));
                    if (property != null)
                    {
                        if (reader[i] == null || DBNull.Value.Equals(reader[i]))
                        {
                            //set value with null only
                            property.SetValue(entity, null, null);
                        }
                        else
                        {
                            //change type nullable to regular type (sample. int? => int)
                            Type t = property.PropertyType;
                            t = Nullable.GetUnderlyingType(t) ?? t;

                            //set value with regular type
                            property.SetValue(entity, Convert.ChangeType(reader[i], t, null), null);
                        }
                    }
                }
                list.Add(entity);
            }
            reader.Close();

            return list;
        }
        public static DataTable FillDataTable(MySqlCommand oCmd)
        {
            using (MySqlDataReader oReader = oCmd.ExecuteReader())
            {
                using (DataTable dataTable = new DataTable() )
                {
                    dataTable.Load(oReader);
                    oReader.Close();
                    return dataTable;
                }
            }
        }       

        public static string ConnectionName(string key)
        {
            string appSetting = ConfigurationManager.AppSettings[key];
            return ConfigurationManager.ConnectionStrings[appSetting].ConnectionString;
        }

        public static string dateTostring(DateTime? data)
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

        public static DateTime? stringTodate(string eXPIRE_ON_STR)
        {
            System.Globalization.CultureInfo _cultureTHInfo = new System.Globalization.CultureInfo("th-TH");
            DateTime? dt = null;
            if (!string.IsNullOrEmpty(eXPIRE_ON_STR))
                dt = DateTime.ParseExact(eXPIRE_ON_STR, "dd/MM/yyyy", _cultureTHInfo);
            return dt;
        }
    }
}
