using System.Security.Claims;
using Application.Features.Users.Queries;
using Domain.Errors;
using Domain.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpGet("GetUserByToken")]
    public async Task<IActionResult> GetUserByToken()
    {
        var userIdClaim = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value;

        if(userIdClaim is null) return Ok(Result.Failure(UserErrors.UserNotFound));

        return Ok(await mediator.Send(new GetUserById(new Guid(userIdClaim))));
    }
}
