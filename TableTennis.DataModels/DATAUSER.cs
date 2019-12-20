using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
    public class DATAUSER
    {
        public long? ID { get; set; }
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string USER_SURNAME { get; set; }
        public string EMAIL { get; set; }
        public int? ROLE_ID { get; set; }
        public string ROLE_DESCRIPTION { get; set; }
        public bool isAdmin { get; set; } = true;
        public string PHONE { get; set; }
        public DateTime? BIRTH_DATE { get; set; }
        public string BIRTH_DATE_STR { get; set; }
        public string GENDER { get; set; }
        
        public string TEAM { get; set; }
    }
}
