using Api.Repositories;
using Api.Services;
using ASG.Api2;
using ASG.Api2.Results;
using ASG.ApiService.Repositories;
using ASG.ApiService.Utilities;
using AutoMapper;
using Dto;
using Dto.Request;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Utilities;


namespace Api.Controllers
{
    [ServiceFilter(typeof(SubscriptionCheckFilter))]
    [ApiController]
    [Route("api/[controller]")]
    //[OutputCache(PolicyName = "CustomPolicy", Tags = new[] { "requests" })]
    public class RequestController(
        IRequestRepository requestRepository,
        IEventRequestRepository eventRequestRepository,
        IModelAiRepository modelAiRepository,
        IModelGatewayRepository modelGatewayRepository,
        ISubscriptionRepository subscriptionRepo,
        TokenService tokenService,
        TrackSubscription trackSubscription,
        IUserClaims userClaims,
        IMapper mapper
        ) : ControllerBase
    {
        [EndpointSummary("Get all Requests")]
        [HttpGet(Name = "GetRequests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<RequestResponse>>> GetAll()
        {
            var items = await requestRepository.GetAllAsync();
            var response = mapper.Map<List<RequestResponse>>(items);
            //return Ok(new BaseResponseModel { Success = true, Data = data });
            return Ok(response);
        }

        [EndpointSummary("Get one")]
        [HttpGet("{id}", Name = "GetRequest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RequestResponse>> GetOne(string id)
        {
            var item = await requestRepository.GetByAsync(r => r.Id == id);
            var response = mapper.Map<RequestResponse>(item);

            return Ok(response);
        }

        //[ServiceFilter(typeof(SubscriptionCheckFilter))]
        [EndpointSummary("Create a request")]
        [HttpPost(Name = "CreateRequest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Create(RequestCreate model)
        {

            var subscription = await subscriptionRepo.GetByAsync(s => s.UserId == userClaims.UserId, setInclude: u => u.Include(s => s.Plan));
            var planService = subscription!.Plan!.PlanServices.FirstOrDefault(ps => ps.ServiceId == model.ServiceId);

            if (trackSubscription.IsSubscribe && planService == null)
                return BadRequest(Result.NotFound("The service that you try to connect not in your plan"));

            trackSubscription.NumberRequests = planService!.NumberRequests;
            trackSubscription.CurrentNumberRequests = await requestRepository.GetCount(userClaims.UserId, model.ServiceId);

            if (!trackSubscription.IsAllowed) return StatusCode(StatusCodes.Status402PaymentRequired, new ProblemDetails { Title = "Requests Ended", Detail = "You have exhausted all allowed subscription requests." });
            var service = planService.Service;
            var modelAi = await modelAiRepository.GetByAsync(m => m.Id == service.ModelAiId);
            var modelGateway = await modelGatewayRepository.GetDefault();
            //var serviceResult = await serviceService.GetById(model.ServiceId);
            //if (serviceResult.IsFailed)
            //{ 
            //    return NotFound(serviceResult.Errors);
            //}

            Entities.Request request = new()
            {
                Status = RequestTypes.Pending.GetDisplayName(),
                Question = model.Value,
                ModelGateway = modelGateway.Url,
                ModelAi = modelAi.Name,
                UserId = userClaims.UserId,
                ServiceId = service.Id,
                SubscriptionId = subscription.Id
            };

            try
            {
                var eventRequest = new EventRequest()
                {
                    Status = RequestTypes.Pending.GetDisplayName(),
                    RequestId = request.Id,
                    Details = $"Request has been created for {request.ModelGateway}/{request.ModelAi}/{service.AbsolutePath}."
                };
                request.Events.Add(eventRequest);
                await requestRepository.CreateAsync(request);
                //if (await requestRepository.CreateAsync(request) != null)
                //{
                //    var eventReq = await eventRequestRepository.CreateAsync(new EventRequest()
                //    {
                //        Status = RequestTypes.Pending.GetDisplayName(),
                //        RequestId = request.Id,
                //        Details = $"Request has been created for {request.ModelGateway}/{request.ModelAi}/{service.AbsolutePath}."
                //    });

                //    var result = mapper.Map<RequestResponse>(request);
                //    result.EventId = eventReq!.Id;
                //}
                //return Result.Fail("Request faild");
                //if (result.IsFailed) return Conflict(result.Errors);
                var token = tokenService.GenerateTemporaryToken(request.Id, eventRequest.Id, userClaims.UserId);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Problem(ex));
            }


        }

        [EndpointSummary("Check if requests allowed")]
        [HttpGet("allowed/{serviceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RequestAllowed>> Allowed(string serviceId)
        {
            var subscription = await subscriptionRepo.GetByAsync(s => s.UserId == userClaims.UserId, setInclude: u => u.Include(s => s.Plan));

            var planService = subscription!.Plan!.PlanServices.FirstOrDefault(ps => ps.ServiceId == serviceId);

            if (trackSubscription.IsSubscribe && planService == null)
                return BadRequest(Result.NotFound("The service that you try to connect not in your plan"));
            trackSubscription.NumberRequests = planService!.NumberRequests;
            trackSubscription.CurrentNumberRequests = await requestRepository.GetCount(userClaims.UserId, serviceId);

            return Ok(new RequestAllowed
            {
                NumberRequests = trackSubscription.NumberRequests,
                CurrentNumberRequests = trackSubscription.CurrentNumberRequests,
                Allowed = trackSubscription.IsAllowed
            });
        }


        [EndpointSummary("Delete request")]
        [HttpDelete("{id}", Name = "DeleteRequest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<DeletedResponse>> Delete(string id)
        {
            try
            {
                if (!await requestRepository.Exists(p => p.Id == id))
                {
                    return NotFound(Result.NotFound($"Plan with id {id} not exists"));
                }
                await requestRepository.RemoveAsync(p => p.Id == id);
                return Ok(new DeletedResponse { Id = id, Deleted = true });
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Problem(ex));
            }
        }


    }
}
