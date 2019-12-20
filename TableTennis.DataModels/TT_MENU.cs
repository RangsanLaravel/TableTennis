using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
  public  class TT_MENU
    {
        public int? MENU_ID { get; set; }
        public string MENU_CODE { get; set; }
        public string MENU_DESCRIPTION { get; set; }
        public string MENU_URL { get; set; }
        public DateTime? TERMINATE_DT { get; set; }
 
    }
}
