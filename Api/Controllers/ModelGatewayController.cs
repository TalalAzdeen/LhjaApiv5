using Api.Repositories;
using ASG.Api2.Results;
using AutoMapper;
using Dto;
using Dto.ModelGateway;
using Entities;
using Microsoft.AspNetCore.Mvc;


namespace ASG.Api2.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    //[OutputCache(PolicyName = "CustomPolicy", Tags = new[] { "Model Gateways" })]
    public class ModelGatewayController(IModelGatewayRepository repository, IMapper mapper) : ControllerBase
    {
        [EndpointSummary("Get all Model Gateway")]
        [HttpGet(Name = "GetModelGatways")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ModelGatewayResponse>>> GetAll()
        {
            var items = await repository.GetAllAsync();
            var result = mapper.Map<List<ModelGatewayResponse>>(items);
            //return Ok(new BaseResponseModel { Success = true, Data = data });
            return Ok(result);
        }

        [EndpointSummary("Get one")]
        [HttpGet("{id}", Name = "GetModelGateway")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ModelGatewayResponse>> GetOne(string id)
        {
            var item = await repository.GetByAsync(m => m.Id == id);
            var result = mapper.Map<ModelGatewayResponse>(item);
            return Ok(result);
        }

        [EndpointSummary("Create a Model Gateway")]
        [HttpPost(Name = "CreateModelGateway")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ModelGatewayResponse>> Create(ModelGatewayCreate model)
        {
            try
            {
                var item = mapper.Map<ModelGatewayCreate, ModelGateway>(model);
                var result = await repository.CreateAsync(item);
                return CreatedAtAction(nameof(GetOne), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ProblemDetails { Type = ex.GetType().FullName, Title = ex.Message, Detail = ex.InnerException?.Message });
            }
        }



        [EndpointSummary("Update Model Gateway")]
        [HttpPut("{id}", Name = "UpdateModelGateway")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ModelGatewayResponse>> Update(string id, [FromBody] ModelGatewayUpdate model)
        {
            try
            {
                var modelGateway = await repository.GetByAsync(r => r.Id == id);
                if (modelGateway == null)
                {
                    return NotFound(Result.NotFound("Record not found make sure that id is correct."));
                }

                modelGateway.Token = model.Token;
                modelGateway.Name = model.Name;
                modelGateway.Url = model.Url;

                await repository.UpdateAsync(modelGateway);
                var result = mapper.Map<ModelGateway, ModelGatewayResponse>(modelGateway);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Problem(ex));
            }
        }

        [HttpPut("default/{id}", Name = "DefaultModelGateway")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MakeDefault(string id)
        {
            var modelGateway = await repository.GetByAsync(r => r.Id == id);
            if (modelGateway == null)
            {
                return NotFound(Result.NotFound("Record not found make sure that id is correct."));
            }

            if (!modelGateway.IsDefault)
            {
                var defaultModel = await repository.GetByAsync(r => r.IsDefault == true);
                defaultModel.IsDefault = false;
                await repository.UpdateAsync(defaultModel);

                modelGateway.IsDefault = true;
                await repository.UpdateAsync(modelGateway);
            }
            return Ok();
        }

        [EndpointSummary("Delete Model Gateway")]

        [HttpDelete("{id}", Name = "DeleteModelGateway")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DeletedResponse>> Delete(string id)
        {
            try
            {
                if (!await repository.Exists(p => p.Id == id))
                {
                    return NotFound($"Model Gateway with id {id} not exists");
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
