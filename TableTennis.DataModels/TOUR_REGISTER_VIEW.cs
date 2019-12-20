using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
    public class TOUR_REGISTER_VIEW
    {
        public int? ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public DateTime BIRTH_DATE { get; set; }
        public char? GENDER { get; set; }
        public string TH_EVENT_NAME { get; set; }
        public decimal? PAY { get; set; }
        public string TEAM { get; set; }
        public int? REG_ID { get; set; }
        public string TEAM_REF { get; set; }
        public string GROUP_NAME { get; set; }
        public string EXPRIE { get; set; }
        public int? RANK { get; set; }
        public decimal? POINT { get; set; }
        public bool EXPRIE_B
        {
            get { return bool.Parse(EXPRIE); }
        }
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

        public string BIRTH_YEAR_STR
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
