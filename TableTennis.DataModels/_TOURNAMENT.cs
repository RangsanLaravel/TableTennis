using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
    public class _TOURNAMENT : TOURNAMENT
    {
        public string TOUR_START_ON_STR { get; set; }
        public string TOUR_END_ON_STR { get; set; }
        public string EXPIRE_ON_STR { get; set; }
    }
}
