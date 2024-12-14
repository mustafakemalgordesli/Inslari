using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Domain.Abstractions;

namespace Domain.Result;

public class Result
{
    protected Result(bool isSuccess, string message = "", Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("InvalidError", nameof(error));
        }
        Message = message;
        IsSuccess = isSuccess;
        Error = error;
    }
    protected Result()
    {
        IsSuccess = false;
        Error = Error.None;
    }

    public bool IsSuccess { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Message { get; }

    public bool IsFailure => !IsSuccess;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Error Error { get; }

    public static Result Success() => new(true, error: Error.None);
    public static Result Success(string message) => new(true, message, Error.None);

    public static Result Failure(Error error) => new(false, error: error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error: error)
    {
        _value = value;
    }

    protected Result(Error error) : base(false, error: error)
    {
        _value = default;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public TValue? Value => IsSuccess
        ? _value is null ? throw new Exception("InvalidError") : _value
        : default;


    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure(new Error("NullError", "Value cannot be null", Enums.ErrorTypes.Failure));

    public static Result<TValue> ValidationFailure(Error error) =>
        new(default, false, error);

    public static Result<TValue> Success(TValue value) =>
        new(value, true, Error.None);

    public static new Result<TValue> Failure(Error error) =>
        new(error);
}
