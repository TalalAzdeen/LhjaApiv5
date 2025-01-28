using Dto.PlanServices;
using System.ComponentModel.DataAnnotations;

namespace Dto.Plan
{

    public class PlanFeatureCreate
    {
        public bool Active { get; set; }

        public Dictionary<string, string>? Name { get; set; }
        public Dictionary<string, string>? Description { get; set; }

    }
    public class PlanCreate
    {
       

        [Required(ErrorMessage = "Price id is Required")]
        public decimal Price { get; set; }
        public required  Dictionary<string, string> Name { get; set; }
        public  Dictionary<string, string>? Description { get; set; }
        public required double Amount { get; set; }

        
        public List<PlanFeatureCreate>? Features { get; set; }
      


        


    }

}
