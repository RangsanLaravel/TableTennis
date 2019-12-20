using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
    public class CAT_MAP_TOUR
    {       
        public int? ID { get; set; }
        public int? CAT_ID { get; set; }
        public int? TOUR_ID { get; set; }
        public int? NUM_OF_PLY { get; set; }
        public string DESCRIPTION { get; set; }
    }
}
