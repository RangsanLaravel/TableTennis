using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
 public   class TT_LOGIN
    {
        public TT_USER dataUser { get; set; }
        public List<TT_MENU> menu_show { get; set; }
        public List<TT_ACCESS_TYPE> user_access { get; set; }

        public bool isLogin { get; set; }
      
    }
}
