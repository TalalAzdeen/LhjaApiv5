using Dto.PlanServices;
using System.ComponentModel;

namespace Dto.Plan
{
    public class PlanUpdate
    {
        [DefaultValue("true")]
        public bool Active { get; set; }
        [DefaultValue("false")]
        public bool ReLoadFromStripe { get; set; }

        public PlanServicesUpdate[] planServices { get; set; }
    }
}
