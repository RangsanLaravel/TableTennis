using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
    public class REGISTER
    {
        public MANAGER manager { get; set; }
        public List<CAT_MAP_TOUR> _catMapTour { get; set; }
        public TOURNAMENT tournament { get; set; }
        public List<EVENT> _event { get;set; } 
        public CAT_MAP_TOUR catMapTour { get; set; }
        public List<SL_TEAM> slTeam { get; set; }
    }
}
