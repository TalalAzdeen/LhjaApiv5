using Api.Repositories;
using Api.Services;
using Dto.Event;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using System.Text.Json;
using Utilities;


namespace ASG.Api2.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    //[OutputCache(PolicyName = "CustomPolicy", Tags = new[] { "requests" })]
    public class EventController(
        IEventRequestRepository eventRequestRepository,
        IRequestRepository requestRepository,
        IServiceRepository serviceRepository,
        EncryptionService encryptionService,
        TokenListService tokenBlacklistService,
        TokenService tokenService) : ControllerBase
    {
        [EndpointSummary("Get all Requests")]
        [HttpPost(Name = "CreateEvent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Create(TokenValidationRequest tokenValidation)
        {
            //if (tokenBlacklistService.IsTokenBlacklisted(tokenValidation.Token))
            //{
            //    return Unauthorized(new { message = "Token has been revoked" });
            //}

            var requestId = "";
            try
            {
                //(string publicKey, string privateKey) = encryptionService.GenerateKeys();
                //return Ok(new { publicKey, privateKey });
                var result = tokenService.ValidateToken(tokenValidation.Token);
                if (result.IsFailed)
                {
                    return BadRequest(result.Errors);
                }
                // التحقق من أن RequestId المرسل موجود في التوكن
                var claims = result.Value.Claims.ToList();
                requestId = claims.FirstOrDefault(c => c.Type == "RequestId")?.Value;
                var eventId = claims.FirstOrDefault(c => c.Type == "EventId")?.Value;

                var request = await requestRepository.GetByAsync(r => r.Id == requestId, true, r => r.Include(r => r.Events));
                if (request == null)
                {
                    return Unauthorized("Invalid RequestId");
                }

                var eventRequest = request.Events.FirstOrDefault(e => e.Id == eventId);
                if (eventRequest == null)
                {
                    return Unauthorized("Event not belong to this request");
                }

                var eventReq = request.Events.FirstOrDefault(e => e.Status == RequestTypes.Processing.GetDisplayName());
                //if (request.Status != RequestTypes.Processing.GetDisplayName())
                //if (eventReq == null)
                //{
                request.Status = RequestTypes.Processing.GetDisplayName();
                request.UpdatedAt = DateTime.UtcNow;

                eventReq = new EventRequest()
                {
                    Status = RequestTypes.Processing.GetDisplayName(),
                    RequestId = request.Id,
                    Details = "Request processing."
                };
                request.Events.Add(eventReq);
                //}

                request.UpdatedAt = DateTime.UtcNow;
                await requestRepository.SaveAsync();

                if (eventReq == null)
                    return Conflict(new { Success = false, Details = "Con not create event for this request" });

                var service = await serviceRepository.GetByAsync(s => s.Id == request.ServiceId);
                //var service = request.Service;
                var json = JsonSerializer.Serialize(new { EventId = eventReq.Id, service.Token, request.Question });
                //var serviceToken = $"{service.Token},{eventReq.Id}";
                string encryptedToken = encryptionService.Encrypt(json, tokenValidation.PublicKey);

                //string decryptedToken = encryptionService.Decrypt(encryptedToken, tokenValidation.PrivateKey);
                return Ok(new { Success = true, Token = encryptedToken });
            }
            catch (Exception ex)
            {
                if (requestId != "")
                {
                    var eventReq = await eventRequestRepository.CreateAsync(new EventRequest()
                    {
                        Status = RequestTypes.Failed.GetDisplayName(),
                        RequestId = requestId!,
                        Details = $"{ex.Message}---{ex.InnerException?.Message}"
                    });
                }

                return BadRequest(new { Success = false, Details = ex.Message });
            }
        }


        [EndpointSummary("Create Event id")]
        [HttpPost("result")]
        public async Task<ActionResult> Result(ResultRequest resultRequest)
        {
            var requestId = "";
            try
            {
                var result = tokenService.ValidateToken(resultRequest.Token);
                if (result.IsFailed)
                {
                    return BadRequest(result.Errors);
                }
                // التحقق من أن RequestId المرسل موجود في التوكن
                var claims = result.Value.Claims.ToList();
                requestId = claims.FirstOrDefault(c => c.Type == "RequestId")?.Value;

                var request = await requestRepository.GetByAsync(e => e.Id == requestId, true);
                if (request == null)
                    return BadRequest(new { Success = false, Details = "Incorrect request" });

                if (resultRequest.Status == RequestTypes.Processing.GetDisplayName())
                {
                    request.Status = RequestTypes.Succeed.GetDisplayName();
                }
                else
                {
                    request.Status = RequestTypes.FailedFromServer.GetDisplayName();
                }

                request.Answer = resultRequest.Result;
                request.Events.Add(new EventRequest { RequestId = request.Id, Status = request.Status, Details = resultRequest.Result });
                await requestRepository.SaveAsync();

                var expirationTime = DateTime.UtcNow.AddDays(1); // وقت انتهاء التوكن
                //tokenBlacklistService.AddTokenTolist(resultRequest.Token, expirationTime);

                return Ok();
            }
            catch (Exception ex)
            {
                if (requestId != "")
                {
                    var eventReq = await eventRequestRepository.CreateAsync(new EventRequest()
                    {
                        Status = RequestTypes.Failed.GetDisplayName(),
                        RequestId = requestId!,
                        Details = $"{ex.Message}---{ex.InnerException?.Message}"
                    });
                }

                return BadRequest(new { Success = false, Details = ex.Message });
            }
        }
    }
}
