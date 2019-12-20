using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SPUtility
{
    public class MySqlParam
    {
        public MySqlParam(string paramName ,object value,MySqlDbType sqlDbType)
        {
            ParamName = paramName;
            Value = value;
            DbType = sqlDbType;
        }

        public string ParamName { get; private set; }
        public object Value { get; private set; }
        public MySqlDbType DbType { get; private set; }
    }
}
