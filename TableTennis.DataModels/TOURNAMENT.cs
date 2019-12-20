using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
    public class TOURNAMENT
    {
        public int? ID { get; set; }
        public string CODE { get; set; }
        public string EN_TOUR_NAME { get; set; }
        public string TH_TOUR_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string TOUR_ADDRESS { get; set; }
        public string TOUR_POINT { get; set; }
        public DateTime? CREATE_ON { get; set; }
        public string CREATE_BY { get; set; }
        public DateTime? MODIFY_ON { get; set; }
        public string MODIFY_BY { get; set; }
        public DateTime? EXPIRE_ON { get; set; }
        public char? TENINATE_FLG { get; set; }
 	    public DateTime? TOUR_START_ON { get; set; }
        public DateTime? TOUR_END_ON { get; set; }
        public string EXPRIE { get; set; }
        public bool EXPRIE_B
        {
            get
            {
                try
                {
                    return bool.Parse(EXPRIE);
                }
                catch 
                {

                    return false;
                }
               
            }
        }
        public List<string> img { get; set; }
        public int? RANKING_ID { get; set; }
    }
}
