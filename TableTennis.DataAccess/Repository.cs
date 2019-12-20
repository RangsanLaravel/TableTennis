using DataAccessUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataAccess
{
    public class Repository:RepositoryBase
    {
        public Repository(string connectionName) 
        {
            base.Repository(connectionName);
        }
    }
}
