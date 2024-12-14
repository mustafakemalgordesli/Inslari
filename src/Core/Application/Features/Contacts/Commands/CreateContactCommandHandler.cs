using Application.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Messages;
using Domain.Repositories;
using Domain.Result;
using Domain.UnitOfWork;
using FluentValidation;
using Mapster;

namespace Application.Features.Contacts.Commands;

public class CreateContactCommand : ICommand<Result>
{
    public Guid UserId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string? Message { get; set; }
}

public class RegisterCommandValidator : AbstractValidator<CreateContactCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(c => c.FullName)
            .MaximumLength(255);
    }
}

public class CreateContactCommandHandler(IContactRepository contactRepository, IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateContactCommand, Result>
{
    public async Task<Result> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(u => u.Id ==  request.UserId && u.IsActive == true);

        if (user is null) return Result.Failure(UserErrors.UserNotFound);

        var contact = contactRepository.Add(request.Adapt<Contact>());

        var rowAffected =  await unitOfWork.SaveChangesAsync(cancellationToken);

        if(rowAffected == 0) return Result.Failure(ContactErrors.MessageNotDelivered);

        return Result.Success(AppMessages.ContactDelivered);
    }
}
