using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
    public class MANAGER
    {
        public long? ID { get; set; }
        public string MANAGER_CODE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public char? GENDER { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
        public string TEAM { get; set; }
        public DateTime? CREATE_ON { get; set; }
        public DateTime? MODIFY_ON { get; set; }
        public string MODIFY_BY { get; set; }
        public char? TEMINATE_FLG { get; set; }
        public DateTime? TOUR_START_ON { get; set; }
        public DateTime? TOUR_END_ON { get; set; }
    }
}
