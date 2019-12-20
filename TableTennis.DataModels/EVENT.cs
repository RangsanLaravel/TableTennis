using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
    public class EVENT
    {
        public int? ID { get; set; }
        public string CODE { get; set; }
        public string EN_EVENT_NAME { get; set; }
        public string TH_EVENT_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public DateTime? CREATE_ON { get; set; }
        public string CREATE_BY { get; set; }
        public DateTime? MODIFY_ON { get; set; }
        public string MODIFY_BY { get; set; }
        public char? TENINATE_FLG { get; set; }
	    public bool TOUR_SELECT { get; set; }     
        public string GEN_CODE { get; set; }
        public decimal? PRICE { get; set; } = 0;
        public string RANK_CODE { get; set; }

    }
}
