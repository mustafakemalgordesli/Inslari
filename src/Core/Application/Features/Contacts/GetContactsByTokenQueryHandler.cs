using Domain.Entities;
using Domain.Errors;
using System.Security.Claims;
using Domain.Repositories;
using Domain.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Application.Responses;
using Mapster;

namespace Application.Features.Contacts;

public record GetContactByTokenQuery(int page = 1, int pageSize = 10) : IRequest<Result<PaginatedResult<ContactResponse>>>;

public class GetContactsByTokenQueryHandler(IContactRepository contactRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetContactByTokenQuery, Result<PaginatedResult<ContactResponse>>>
{
    public async Task<Result<PaginatedResult<ContactResponse>>> Handle(GetContactByTokenQuery request, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var user = await userRepository.GetAsync(u => u.Id == userId);

        if (userId == null || user == null) return Result<PaginatedResult<ContactResponse>>.Failure(UserErrors.UserNotFound);

        var paginatedData = await contactRepository.GetPaginatedAsync(page: request.page, pageSize: request.pageSize, predicate: c => c.UserId == user.Id);

        return Result<PaginatedResult<ContactResponse>>.Success(paginatedData.Adapt<PaginatedResult<ContactResponse>>());
    }

    private Guid? GetCurrentUserId()
    {
        return Guid.TryParse(
            httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            out Guid parsed)
            ? parsed : null;
    }
}
