using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
    public class TOUR_REGISTER
    {
        public int? ID { get; set; }
        public int? TOUR_MAP_ID { get; set; }
        public int? PLAYER_ID { get; set; }
        public int? MANAGER_ID { get; set; }
        public DateTime? CREATE_ON { get; set; }
        public string CREATE_BY { get; set; }
        public DateTime? MODIFY_ON { get; set; }
        public string MODIFY_BY { get; set; }
        public char? TENINATE_FLG { get; set; }
        public int? CATEGORY_ID { get; set; }
        public string TEAM_REF { get; set; }
        public string GROUP_NAME { get; set; }
    }
}
