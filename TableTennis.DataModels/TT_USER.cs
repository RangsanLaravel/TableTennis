using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
  public  class TT_USER 
    {
        public int? ID { get; set; }
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string USER_SURNAME { get; set; }
        public string EMAIL { get; set; }
        public string PASSWORD { get; set; }
        public int? ROLE_ID { get; set; }
        public DateTime? TMN_DT { get; set; }
    }
}
