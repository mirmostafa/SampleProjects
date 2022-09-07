using System.Diagnostics.CodeAnalysis;

namespace AutoMapperConsoleApp.Results;

public class Result<TValue> : ResultBase
{
    public Result(in TValue value, in object? status = null, in string? message = null) : base(status, message)
        => Value = value;

    public Result(in TValue value, object? status, string? fullMessage) : base(status, fullMessage)
        => Value = value;

    public Result(in TValue value, object? status, [DisallowNull] Exception exception) : base(status, exception)
        => Value = value;

    public static Result<TValue?> Fail
        => CreateFail(error: -1);

    public TValue Value { get; }

    [return: NotNull]
    public static Result<TValue?> CreateFail(in string? message = null, in TValue? value = default, in object? error = null)
        => new(value, error ?? -1, message);

    [return: NotNull]
    public static Result<TValue> CreateSuccess(in TValue value, in string? message = null, in object? status = null)
        => new(value, status, message);

    public static Result<TValue> From<TValue1>([DisallowNull] in Result<TValue1> other, TValue value)
        => ResultBase.From(other, new Result<TValue>(value));

    public static Result<TValue> From([DisallowNull] in Result other, in TValue value)
        => ResultBase.From(other, new Result<TValue>(value));

    public static implicit operator bool(in Result<TValue?> result)
        => result.IsSucceed;

    public static implicit operator Result(in Result<TValue> result)
        => result.ConvertTo();

    public static implicit operator TValue(in Result<TValue> result)
        => result.Value;

    public static Result<TValue> New(TValue item)
        => new(item);

    public Result ConvertTo()
        => IsSucceed ? Result.CreateSuccess(Message, Status) : Result.CreateFail(Message, Status);

    public void Deconstruct(out object? Status, out string? Message, out TValue Value)
        => (Status, Message, Value) = (this.Status, this.Message, this.Value);

    public void Deconstruct(out bool isSucceed, out TValue Value)
        => (isSucceed, Value) = (IsSucceed, this.Value);

    public bool Equals(Result<TValue?> other)
        => other is not null && (other.Status, other.IsSucceed) == (Status, IsSucceed) && (other.Value?.Equals(Value) ?? Value is null);

    public Result<TValue1> ToResult<TValue1>(TValue1 value)
        => From(this, new Result<TValue1>(value));

    public Result<TValue1> ToResult<TValue1>(Func<Result<TValue>, TValue1> action)
        => From(this, new Result<TValue1>(action(this)));

    public Task<Result<TValue>> ToTask()
                => Task.FromResult(this);

    public Result<TValue1> With<TValue1>(in TValue1 value1)
        => Result<TValue1>.From(this, value1);
}