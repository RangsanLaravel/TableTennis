using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
   public class TOUR_MAPBASE
    {
        public int? ID { get; set; }
        public int? TOURNAMENT_ID { get; set; }
        public int? EVENT_ID { get; set; }
        public decimal? PAY { get; set; }
        public DateTime? CREATE_ON { get; set; }
        public string CREATE_BY { get; set; }
        public DateTime MODIFY_ON { get; set; }
        public string MODIFY_BY { get; set; }
        public string TENINATE_FLG { get; set; }
       
    }
}
