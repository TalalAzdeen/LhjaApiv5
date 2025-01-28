using Api.Services;
using Dto.Service;
using Dto.Setting;
using Microsoft.AspNetCore.Mvc;


namespace ASG.Api2.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    //[OutputCache(PolicyName = "CustomPolicy", Tags = new[] { "settings" })]
    public class SettingController(SettingService setting) : ControllerBase
    {
        [EndpointSummary("Get all Setting")]
        [HttpGet]
        public async Task<ActionResult<List<object>>> GetAll()
        {
            var data = await setting.GetAll();
            //return Ok(new BaseResponseModel { Success = true, Data = data });
            return Ok(data.ValueOrDefault);
        }

        [EndpointSummary("Get one")]
        [HttpGet("{name}")]
        public async Task<ActionResult<ServiceResponse>> GetOne(string name)
        {
            var result = await setting.GetBy(s => s.Name == name);
            return Ok(result.ValueOrDefault);
        }

        [EndpointSummary("Create a setting")]
        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> Create(SettingCreate model)
        {
            var result = await setting.CreateAsync(model);
            return result.IsFailed ? Conflict(result.Errors)
                : CreatedAtAction(nameof(GetOne), new { name = result.Value?.Name }, result.Value);
        }

        [EndpointSummary("Update setting")]
        [HttpPut("{name}")]
        public async Task<ActionResult> Update(string name, SettingUpdate model)
        {
            var result = await setting.UpdateAsync(name, model);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors);
        }

        [EndpointSummary("Delete setting")]
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var result = await setting.Delete(name);
            return result.IsSuccess ? Ok() : NotFound(result.Errors);
        }

    }
}
