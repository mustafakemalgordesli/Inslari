using Application.Responses;
using Domain.Errors;
using Domain.Repositories;
using Domain.Result;
using Mapster;
using MediatR;

namespace Application.Features.Users.Queries;

public sealed record GetUserById(Guid Id) : IRequest<Result<UserResponse>>;
public class GetUserByIdQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserById, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(GetUserById request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(u => u.Id == request.Id && u.IsActive == true);

        if (user == null) return Result<UserResponse>.Failure(UserErrors.UserNotFound);

        var userResponse = user.Adapt<UserResponse>();

        return Result<UserResponse>.Success(userResponse);
    }
}