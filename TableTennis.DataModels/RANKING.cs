using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
  public  class RANKING
    {
        public int ID { get; set; }
        public string DESC { get; set; }
        public DateTime? CREATE_ON { get; set; }
        public string CREATE_BY { get; set; }
        public DateTime? MODIFY_ON { get; set; }
        public string MODIFY_BY { get; set; }
        public string TERNINATE_FLG { get; set; }

    }
}
