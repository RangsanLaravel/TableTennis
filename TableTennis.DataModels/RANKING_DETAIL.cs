using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
    public class RANKING_DETAIL
    {
        public int? ID { get; set; }
        public long? RANKING_ID { get; set; }
        public string RANK_CODE { get; set; }
        public int? PLAYER_ID { get; set; }
        public string POINT { get; set; }
        public string BELONG_TO { get; set; }
        public int? RANK { get; set; }
        public string NAME { get; set; }

    }
}
