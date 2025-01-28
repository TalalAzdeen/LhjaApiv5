using Api.Repositories;
using ASG.Api2.Results;
using ASG.ApiService.Repositories;
using AutoMapper;
using Dto.Role;
using Dto.Subscription;
using Dto.User;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StripeGateway;

namespace Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController(
        IUserRepository userRepository,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        IMapper mapper,
        IStripeCustomer stripeCustomer,
        IUserClaims userClaims
        ) : Controller
    {


        [HttpGet(Name = "GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<UserResponse>>> GetUsers()
        {
            var user = await userRepository.GetAllAsync(null, setInclude: u => u.Include(s => s.Subscription));
            var response = mapper.Map<List<UserResponse>>(user);

            return Ok(response);
        }


        [HttpGet("{id}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserResponse>> GetUser(string id)
        {
            var user = await userRepository.GetByAsync(u => u.Id == id, setInclude: u => u.Include(s => s.Subscription));
            var response = mapper.Map<UserResponse>(user);

            return Ok(response);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("create-session")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<SubscriptionResponse>>> CreateSession()
        {
            var user = await userRepository.GetByAsync(null);
            var result = await stripeCustomer.CreateCustomerSession(user.CustomerId);

            return Ok(result);
        }

        [HttpPost("assignRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> AssignRole(RoleAssign roleAssign)
        {
            try
            {
                //var user = await userManager.FindByEmailAsync(roleAssign.Email);
                ApplicationUser user = (await userManager.FindByIdAsync(userClaims.UserId!))!;
                var roles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, roles);

                var role = await roleManager.FindByIdAsync(roleAssign.RoleId);
                if (role != null)
                {
                    IdentityResult result = await userManager.AddToRoleAsync(user, role.Name!);
                    if (result.Succeeded)
                    {
                        var claims = await roleManager.GetClaimsAsync(role);
                        //await userManager.AddClaimsAsync
                        await userManager.UpdateAsync(user);
                        await signInManager.RefreshSignInAsync(user);
                        var response = mapper.Map<UserResponse>(user);
                        return Ok(response);
                    }
                    else
                        return Conflict(Result.Problem("Assign Role", "Connot assign this role to user"));
                }
                else
                {
                    return NotFound(Result.NotFound($"There is no role with id {roleAssign.RoleId}"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Problem(ex));
            }
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("add-claim")]
        public async Task<IActionResult> AddClaim(string type, string value)
        {
            //var userRepo = await userRepository.InitializeAsync();
            //await userRepo.SaveClaimAsync(new Claim(type, value));
            //await userRepo.RefreshAsync();
            return Ok();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("delete-claim/{type}")]
        public async Task<IActionResult> DeleteClaims(string type)
        {
            //var userRepo = await userRepository.RemoveClaimAsync(type);
            //await userRepo.RefreshAsync();
            return Ok();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("get-claims")]
        public async Task<ActionResult> GetClaims()
        {
            //var userRepo = await userRepository.InitializeAsync();
            ////var claims = await userRepo.GetClaim();
            //List<Claim>? claims = User.Claims.ToList();


            return Ok(new
            {
                //claims = claims?.Select(x => new { x.Type, x.Value }),
                //claim
            });
        }

    }
}