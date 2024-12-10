using Application.Abstractions;
using Domain.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace Application.Behaviors;

public sealed class TransactionalPipelineBehavior<TRequest, TResponse>(
    ITransactionUnitOfWork unitOfWork,
    ILogger<TransactionalPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITransactionalCommand<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            logger.LogInformation("Beginning transaction for {RequestName}", typeof(TRequest).Name);

            TResponse response = await next();

            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Committed transaction for {RequestName}", typeof(TRequest).Name);

            return response;
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw new TransactionException(ex.Message);
        }
    }
}