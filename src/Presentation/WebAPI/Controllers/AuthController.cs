using Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterCommand request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand request)
        {
            return Ok(await mediator.Send(request));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> LoginUserWithRefreshToken([FromBody] LoginUserWithRefreshToken request)
        {
            return Ok(await mediator.Send(request));
        }

        [Authorize]
        [HttpDelete("logout/{token}")]
        public async Task<IActionResult> Logout(string token)
        {
            return Ok(await mediator.Send(new LogoutCommand(token)));
        }
    }
}
