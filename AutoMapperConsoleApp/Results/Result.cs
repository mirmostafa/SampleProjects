using System.Diagnostics.CodeAnalysis;

using AutoMapperConsoleApp.Validations;

namespace AutoMapperConsoleApp.Results;

public class Result : ResultBase
{
    private static Result? _empty;
    private static Result? _fail;
    private static Result? _success;

    public Result(in object? status = null, in string? fullMessage = null)
        : base(status, fullMessage) { }

    public static Result Empty { get; } = _empty ??= NewEmpty();
    public static Result Fail => _fail ??= CreateFail();
    public static Result Success => _success ??= CreateSuccess();

    public static Result CreateFail(in string? message = null, in object? error = null)
        => new(error ?? -1, message) { IsSucceed = false };

    public static Result CreateSuccess(in string? fullMessage = null, in object? status = null)
        => new(status, fullMessage) { IsSucceed = true };

    public static explicit operator Result(bool b)
        => b ? Success : Fail;

    public static Result From([DisallowNull] in ResultBase other)
        => From(other, new Result());

    public static implicit operator bool(Result result)
            => result.NotNull().IsSucceed;

    public static Result New()
        => new();

    public static Result NewEmpty()
        => New();
}