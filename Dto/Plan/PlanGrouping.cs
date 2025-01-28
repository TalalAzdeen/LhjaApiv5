using Dto.PlanServices;
using System.ComponentModel.DataAnnotations;

namespace Dto.Plan
{
    public class PlanFeatureView
    {
     
        public int Id { get; set; }

      
        public string? Name { get; set; }


        public string? Description { get; set; }
    }
        public class PlanGrouping
    {



        public string ProductId { get; set; }
        public string BillingPeriod { get; set; }
        public double Amount { get; set; }
        public bool Active { get; set; }
         public string? Description { get; set; }
        public required string ProductName { get; set; }
        public List<PlanServicesResponse> Services { get; set; }

        public List<PlanFeatureView> PlanFeatures { get; set; }

    }

    public class PlanView
    {



        public string Id { get; set; }
        public string BillingPeriod { get; set; }
        public double Amount { get; set; }
        public bool Active { get; set; }
        public string? Description { get; set; }
        public  string Name { get; set; }

        public List<PlanFeatureView>? PlanFeatures { get; set; }

    }
}
