using SPUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.BusinessLogic
{
    public class SPConnectionBase
    {
        public string connnectionName;

        public SPConnectionBase()
        {
            this.connnectionName = Utility.ConnectionName("ConnectionName");
        }
    }
}
