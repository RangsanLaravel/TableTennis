using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
  public  class TT_ACCESS_TYPE
    {
        public int ID { get; set; }
        public string ACCESS_CODE { get; set; }
        public string ACCESS_DESCRIPTION { get; set; }
        public DateTime? TMN_DT { get; set; }

    }
}
