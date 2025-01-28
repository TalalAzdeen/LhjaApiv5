using Api.Repositories;
using ASG.Api2.Results;
using AutoMapper;
using Dto;
using Dto.Plan;
using Dto.PlanServices;
using Entities;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StripeGateway;


namespace ASG.Api2.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    //[OutputCache(PolicyName = "CustomPolicy", Tags = new[] { "plans" })]
    public class PlansController(IPlanRepository planRepository,
        IStripePrice stripePrice,
        IStripeProduct stripeProduct,
        IMapper mapper) : ControllerBase
    {
        [EndpointSummary("Get all plans")]
        [HttpGet(Name = "GetPlans")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PlanResponse>>> GetAll()
        {
            var plans = await planRepository.GetAllAsync();

            var planResponse = mapper.Map<List<PlanResponse>>(plans);
            return Ok(planResponse);
        }

        [AllowAnonymous]
        [HttpGet("group", Name = "AsGroup")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PlanView>>> Group(string langauge="ar")
        {


            var result = await planRepository.GetAllAsGroupAsync(langauge);

            //var planServices = mapper.Map<Dto.Plan.PlanFeature[], PlanGrouping[]>(
            //    result.Cast<Dto.Plan.PlanFeature>().ToArray()
            //);

          

            //var listt=new List<Dto.Plan.PlanFeature>();
            //foreach (var item in result.pro)
            //{
            //    // إنشاء كائن جديد لكل عنصر
            //    var cc = new Dto.Plan.PlanFeature
            //    {
            //        Name = HelperTranslation.ConvertTextToTranslationData(item.ProductId)
            //    };

            //    // إضافة الكائن إلى القائمة
            //    listt.Add(cc);
            //}


            return Ok(result);
        }

        [EndpointSummary("Get one")]
        [HttpGet("{id}", Name = "GetPlan")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PlanResponse>> GetOne(string id)
        {
            var plan = await planRepository.GetByAsync(p => p.Id == id);
            var item = mapper.Map<PlanResponse>(plan);
            return Ok(item);
        }

        [EndpointSummary("Create a plan")]
        [HttpPost(Name = "CreatePlan")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PlanResponse>> Create(PlanCreate model)
        {
            try
            {




              

                //var plan = await planRepository.GetByAsync(p => p.Id == model.PriceId);
                //if (plan is not null)
                //{
                //    return Conflict(new ProblemDetails { Title = "Exist", Detail = $"Plan with id {model.PriceId} already exists" });
                //}

                //var price = await stripePrice.GetByIdAsync(model.PriceId);
                //var product = await stripeProduct.GetByIdAsync(price.ProductId);


                //double amount = price.UnitAmount / 100d ?? 0;
                ////plan = new()
                ////{
                ////    Id = price.Id,
                ////    BillingPeriod = price.Recurring.Interval,
                ////    Amount = amount,
                ////    ProductId = product.Id,
                ////    ProductName = HelperTranslation.ConvertTranslationDataToText(model.ProductName),
                ////    Description = HelperTranslation.ConvertTranslationDataToText(model.ProductName),
                ////    Images = product.Images,
                ////};

                //plan = new()
                //{
                //    Id = price.Id,
                //    BillingPeriod = price.Recurring.Interval,
                //    Amount = amount,
                //    ProductId = product.Id,
                //    ProductName = HelperTranslation.ConvertTranslationDataToText(model.Name),
                //    Description = HelperTranslation.ConvertTranslationDataToText(model.Name),
                //    Images = product.Images,
                //};


                //var PlanServices = mapper.Map<PlanServicesCreate[], PlanServices[]>(model.PlanServices);
                //foreach (var planService in PlanServices)
                //{
                //    planService.PlanId = plan.Id;
                //}
                //plan.PlanServices = PlanServices;





               

                ////await sender.Send(new AddPlanCommand(plan));
                //await planRepository.CreateAsync(plan);

                //plan = await planRepository.GetByAsync(p => p.Id == plan.Id);
                //var item = mapper.Map<PlanResponse>(plan);




                return StatusCode(StatusCodes.Status201Created, null);
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails { Title = ex.Message, Detail = ex.InnerException?.Message });
            }

        }


        [EndpointSummary("Update plan")]
        [HttpPut("{id}", Name = "UpdatePlan")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PlanResponse>> Update(string id, PlanUpdate model)
        {
            try
            {

                Plan plan = await planRepository.GetByAsync(p => p.Id == id, setInclude: p => p.Include(p => p.PlanServices));

                if (plan is null)
                {
                    return NotFound(Results.Result.NotFound("Record not found make sure that id is correct."));
                }

                var product = await stripeProduct.GetByIdAsync(plan.ProductId);

                plan.UpdatedAt = DateTime.Now;
                plan.Active = model.Active;
                if (model.ReLoadFromStripe)
                {
                    plan.ProductName = product.Name;
                    plan.Description = product.Description;
                    plan.Images = product.Images;
                }
                foreach (var planServices in model.planServices)
                {
                    var planService = plan.PlanServices.FirstOrDefault(ps => ps.ServiceId == planServices.ServiceId);
                    planService.NumberRequests = planServices.NumberRequests;
                    planService.Scope = planServices.Scope;
                    planService.Processor = planServices.Processor;
                    planService.ConnectionType = planServices.ConnectionType;

                }
                var item = await planRepository.UpdateAsync(plan);
                var result = mapper.Map<PlanResponse>(item);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails { Title = ex.Message, Detail = ex.InnerException?.Message });
            }
        }

        [EndpointSummary("Delete plan")]
        [HttpDelete("{id}", Name = "DeletePlan")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DeletedResponse>> Delete(string id)
        {
            try
            {
                if (!await planRepository.Exists(p => p.Id == id))
                {
                    return NotFound($"Plan with id {id} not exists");
                }
                await planRepository.RemoveAsync(p => p.Id == id);
                return Ok(new DeletedResponse { Id = id, Deleted = true });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_Subscriptions_Plans_PlanId"))
                    return Conflict("This item cannot be deleted because it has one or more subscriptions.");
                return BadRequest(new ProblemDetails { Type = ex.GetType().FullName, Title = ex.Message, Detail = ex.InnerException?.Message });
            }
        }
    }
}
