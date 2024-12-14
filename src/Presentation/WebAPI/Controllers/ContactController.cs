using Application.Features.Contacts;
using Application.Features.Contacts.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateContact([FromBody] CreateContactCommand request)
    {
        return Ok(await mediator.Send(request));
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetContacts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        return Ok(await mediator.Send(new GetContactByTokenQuery(page, pageSize));
    }
}
