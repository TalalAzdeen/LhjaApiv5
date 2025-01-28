using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Api.Controllers.Permission;

//[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class PermissionController : Controller
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public PermissionController(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpGet("GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetAll()
    {

        //var model = new PermissionResponse() { RoleId = roleId };
        //model.RoleId = roleId;
        //var allPermissions = new List<RoleClaimsResponse>();
        //var role = await _roleManager.FindByIdAsync(roleId);
        //string r = "g";
        //if (role.Name == UserRole.SuperVisor) r = "s";


        var permitions = await Permissions.GetFields();

        //var claims = await _roleManager.GetClaimsAsync(role);
        //var allClaimValues = allPermissions.Select(a => a.Value).ToList();
        //var roleClaimValues = claims.Select(a => a.Value).ToList();
        //var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
        //foreach (var permission in allPermissions)
        //{
        //    if (authorizedClaims.Any(a => a == permission.Value))
        //    {
        //        permission.Selected = true;
        //    }
        //}
        //model.RoleClaims = allPermissions;
        return Ok(permitions);
    }

    //[HttpPut("Update")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> Update(PermissionResponse model)
    //{
    //    var role = await _roleManager.FindByIdAsync(model.RoleId);
    //    var claims = await _roleManager.GetClaimsAsync(role);
    //    foreach (var claim in claims)
    //    {
    //        await _roleManager.RemoveClaimAsync(role, claim);
    //    }
    //    var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
    //    foreach (var claim in selectedClaims)
    //    {
    //        await _roleManager.AddPermissionClaim(role, claim.Value);
    //    }
    //    return Ok();
    //}


}
