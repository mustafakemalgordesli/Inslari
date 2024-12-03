using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Domain.Abstractions;

namespace Domain.Result;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("InvalidError", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }
    protected Result()
    {
        IsSuccess = false;
        Error = Error.None;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    protected Result(Error error) : base(false, error)
    {
        _value = default;
    }

    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("InvalidError");


    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure(new Error("NullError", "Value cannat be null", Enums.ErrorTypes.Failure));

    public static Result<TValue> ValidationFailure(Error error) =>
        new(default, false, error);

    public static Result<TValue> Success(TValue value) =>
        new(value, true, Error.None);

    public static new Result<TValue> Failure(Error error) =>
        new(error);
}
