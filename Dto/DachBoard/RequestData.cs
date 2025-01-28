using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.DachBoard
{
    public class RequestData
    {
        public DateTime Time { get; set; }
        public int Requests { get; set; }
        public int Errors { get; set; }
        public string ServiceType { get; set; }
    }
}
