using AutoMapper;
using Dto.Role;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace Api.Controllers.Permission
{
    [Authorize(Roles = "admin", Policy = "CanViewPlan")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(RoleManager<ApplicationRole> roleManager, IMapper mapper) : Controller
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            var roles = await roleManager.Roles
                .Where(s => s.NormalizedName != UserRole.Admin.ToUpper())
                //.Include(r => r.RoleClaims)
                .ToListAsync();
            var result = mapper.Map<List<RoleResponse>>(roles);
            //return Ok(roles);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoleResponse>> GetOne(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            var result = mapper.Map<RoleResponse>(role);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] RoleCreate roleCreate)
        {
            if (roleCreate.Name.ToLower() == UserRole.Admin.ToLower())
            {
                return Conflict(new ProblemDetails { Detail = "Cannot add this role" });
            }
            await roleManager.CreateAsync(new ApplicationRole { Name = roleCreate.Name.Trim() });
            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            var claims = await roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await roleManager.RemoveClaimAsync(role, claim);
            }

            await roleManager.DeleteAsync(role);
            return Ok();
        }


        [HttpPost("assignPermission")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignPermission(RolePermitionAssign rolePermition)
        {
            var role = await roleManager.FindByIdAsync(rolePermition.RoleId);
            await roleManager.AddPermissionClaim(role, rolePermition.Permissions);
            return Ok();
        }
    }

}