using Api.Repositories;
using ASG.Api2.Results;
using AutoMapper;
using Dto;
using Dto.ServiceMethod;
using Entities;
using Microsoft.AspNetCore.Mvc;


namespace ASG.Api2.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    //[OutputCache(PolicyName = "CustomPolicy", Tags = new[] { "Service Methods" })]
    public class ServiceMethodController(IServiceMethodRepository repository, IMapper mapper) : ControllerBase
    {
        [EndpointSummary("Get all Service Method")]
        [HttpGet(Name = "GetServiceMethods")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ServiceMethodResponse>>> GetAll()
        {
            var items = await repository.GetAllAsync();
            var result = mapper.Map<List<ServiceMethodResponse>>(items);
            return Ok(result);
        }

        [EndpointSummary("Get one")]
        [HttpGet("{id}", Name = "GetServiceMethod")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ServiceMethodResponse>> GetOne(string id)
        {
            var item = await repository.GetByAsync(m => m.Id == id);
            var result = mapper.Map<ServiceMethodResponse>(item);
            return Ok(result);
        }

        [EndpointSummary("Create a Service Method")]
        [HttpPost(Name = "CreateServiceMethods")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceMethodResponse>> Create(ServiceMethodCreate model)
        {
            try
            {
                var item = mapper.Map<ServiceMethodCreate, ServiceMethod>(model);
                //item.InputParameters= Json
                await repository.CreateAsync(item);
                var result = mapper.Map<ServiceMethodResponse>(item);
                return CreatedAtAction(nameof(GetOne), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails { Type = ex.GetType().FullName, Title = ex.Message, Detail = ex.InnerException?.Message });
            }
        }

        [EndpointSummary("Update Service Method")]
        [HttpPut("{id}", Name = "UpdateServiceMethods")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<ServiceMethodResponse>> Update(string id, ServiceMethodUpdate model)
        {
            try
            {
                var serviceMethod = await repository.GetByAsync(sm => sm.Id == id);
                if (serviceMethod == null)
                {
                    return NotFound(Result.NotFound("Record not found make sure that id is correct."));
                }
                var item = mapper.Map<ServiceMethodUpdate, ServiceMethod>(model);
                item.Id = id;
                item.ServiceId = serviceMethod.ServiceId;
                await repository.UpdateAsync(item);
                var result = mapper.Map<ServiceMethodResponse>(item);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails { Type = ex.GetType().FullName, Title = ex.Message, Detail = ex.InnerException?.Message });
            }
        }

        [EndpointSummary("Delete Service Method")]
        [HttpDelete("{id}", Name = "DeleteServiceMethods")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DeletedResponse>> Delete(string id)
        {
            try
            {
                if (!await repository.Exists(p => p.Id == id))
                {
                    return NotFound($"Service Method with id {id} not exists");
                }
                await repository.RemoveAsync(p => p.Id == id);
                return Ok(new { Id = id, Deleted = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails { Type = ex.GetType().FullName, Title = ex.Message, Detail = ex.InnerException?.Message });
            }
        }
    }
}
