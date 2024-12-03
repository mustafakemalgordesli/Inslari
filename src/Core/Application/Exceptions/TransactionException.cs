namespace Application.Exceptions;

public class TransactionException : Exception
{
    public TransactionException(string message)
            : base(message)
    { }

    public TransactionException()
           : base("TransactionError")
    { }
}
