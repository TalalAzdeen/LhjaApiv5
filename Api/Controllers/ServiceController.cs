using Api.Repositories;
using ASG.Api2.Results;
using AutoMapper;
using Dto;
using Dto.Service;
using Entities;
using Microsoft.AspNetCore.Mvc;


namespace ASG.Api2.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    //[OutputCache(PolicyName = "CustomPolicy", Tags = new[] { "services" })]
    public class ServiceController(IServiceRepository repository, IMapper mapper) : ControllerBase
    {
        [EndpointSummary("Get all Services")]
        [HttpGet(Name = "GetServices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ServiceResponse>>> GetAll()
        {
            var items = await repository.GetAllAsync();
            var result = mapper.Map<List<ServiceResponse>>(items);
            return Ok(items);
        }

        [EndpointSummary("Get one")]
        [HttpGet("{id}", Name = "GetService")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ServiceResponse>> GetOne(string id)
        {
            var item = await repository.GetByAsync(s => s.Id == id);
            var result = mapper.Map<ServiceResponse>(item);
            return Ok(result);
        }

        [EndpointSummary("Create a service")]
        [HttpPost(Name = "CreateService")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceResponse>> Create(ServiceCreate model)
        {
            try
            {
                var item = mapper.Map<Service>(model);
                await repository.CreateAsync(item);
                var result = mapper.Map<ServiceResponse>(item);
                return CreatedAtAction(nameof(GetOne), new { id = result?.Id }, result);
                //return StatusCode(StatusCodes.Status201Created, item);
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails { Title = ex.Message, Detail = ex.InnerException?.Message });
            }
        }

        [EndpointSummary("Update service")]
        [HttpPut("{id}", Name = "UpdateService")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(string id, ServiceUpdate model)
        {
            Service service = await repository.GetByAsync(p => p.Id == id);
            if (service is null)
            {
                return NotFound(Result.NotFound("Record not found make sure that id is correct."));
            }

            service.Name = model.Name;
            service.Token = model.Token;
            service.ModelAiId = model.ModelAiId;

            var item = await repository.UpdateAsync(service);
            var result = mapper.Map<ServiceResponse>(item);
            return Ok(result);
        }

        [EndpointSummary("Delete service")]
        [HttpDelete("{id}", Name = "DeleteService")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DeletedResponse>> Delete(string id)
        {
            try
            {
                if (!await repository.Exists(p => p.Id == id))
                {
                    return NotFound($"Service with id {id} not exists");
                }
                await repository.RemoveAsync(p => p.Id == id);
                return Ok(new DeletedResponse { Id = id, Deleted = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails { Type = ex.GetType().FullName, Title = ex.Message, Detail = ex.InnerException?.Message });
            }
        }
    }
}
