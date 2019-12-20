using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPUtility
{
    public class JsonResponse : Result
    {
        public object data { get; set; }

        public bool session { get; set; }
    }
}
