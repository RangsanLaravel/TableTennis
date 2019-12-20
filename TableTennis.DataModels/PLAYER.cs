using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
    public class PLAYER
    {
        public int? ID { get; set; }
        public string PLAYER_CODE { get; set; }
        public string SALUTATION { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public char? GENDER { get; set; }        
        public DateTime? BIRTH_DATE { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
        public DateTime? CREATE_ON { get; set; }
        public string CREATE_BY { get; set; }
        public DateTime? MODIFY_ON { get; set; }
        public string MODIFY_BY { get; set; }
        public char? TENINATE_FLG { get; set; }
        
        public string GENDER_DESC
        {
            get
            {
                if (GENDER == 'M')
                    return "ชาย";
                else if (GENDER == 'F')
                    return "หญิง";

                return null;
            }
        }
        public string BIRTH_DATE_STR
        {
            get
            {
                return SPUtility.Utility.dateTostring(BIRTH_DATE);
            }
        }

        public string BIRTH_YEAR
        {
            get
            {
                if (BIRTH_DATE == null)
                    return null;

                CultureInfo ThaiCulture = new CultureInfo("th-TH");
                DateTime dt = (DateTime)BIRTH_DATE;
                return dt.ToString("yyyy", ThaiCulture); 
            }
        }

        public int? AGE
        {
            get
            {
                if (BIRTH_DATE != null)
                {
                    DateTime date = (DateTime)BIRTH_DATE;
                    return DateTime.Now.Year - date.Year;
                }
                return null;   
            }
        }
    }
}
