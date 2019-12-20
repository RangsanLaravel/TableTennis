using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
 public   class TEMP_RANKING
    {
        public int ID { get; set; }
        public int? PLAYER_CODE { get; set; }
        public int? Ranking1 { get; set; }
        public int? Ranking2 { get; set; }
        public string name { get; set; }
        public string year { get; set; }
        public string Affiliation { get; set; }
        public int? POINT { get; set; }
    }
}
