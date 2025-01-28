using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.ModelAi
{
    public   class FilterModelAI
    {
        public string Name { get; set; }
         

        public string? Category { get; set; }
        public string? Language { get; set; }
        public bool? IsStandard { get; set; }
        public string? Gender { get; set; }
        public string? Dialect { get; set; }
        public string? Type { get; set; }
    }
}
