using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableTennis.DataModels
{
    public class DataFile
    {
        public string header { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string size { get; set; }
        public string src { get; set; }
        public string path_file { get; set; }
        public long? image_id { get; set; }
        public int? tour_id { get; set; }
        public string uri_path { get; set; }
    }
}
