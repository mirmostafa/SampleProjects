using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using AutoMapperConsoleApp.Validations;

namespace AutoMapperConsoleApp.Results;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public abstract class ResultBase : IEquatable<ResultBase?>
{
    private string? _message;
    private bool? _isSucceed;

    protected ResultBase(object? status = null, string? message = null)
        => (Status, Message) = (status, message);
    protected ResultBase(object? status, [DisallowNull] Exception exception)
        => (Status, Message) = (status, exception.NotNull().GetBaseException().Message);

    public List<(object? Id, object Message)> Errors { get; } = new();

    public Dictionary<string, object> Extra { get; } = new();

    public bool IsFailure => !IsSucceed;

    public virtual bool IsSucceed
    {
        get => _isSucceed ?? Status is null or 0 or 200 && !Errors.Any();
        init => _isSucceed = value;
    }

    public string? Message { get; }

    public object? Status
    {
        get;
        protected set;
    }

    public static bool operator !=(ResultBase? left, ResultBase? right)
        => !(left == right);

    public static bool operator ==(ResultBase? left, ResultBase? right)
        => EqualityComparer<ResultBase>.Default.Equals(left, right);

    public void Deconstruct(out bool isSucceed, out string? message)
                => (isSucceed, message) = (IsSucceed, Message);

    public override bool Equals(object? obj) =>
        Equals(obj as ResultBase);

    public bool Equals(ResultBase? other) =>
        other is not null && Status == other.Status;

    public override int GetHashCode() =>
        HashCode.Combine(Status, Message, Errors);

    public override string ToString()
    {
        var result = new StringBuilder($"IsSucceed: {IsSucceed}").AppendLine();
        if (!string.IsNullOrEmpty(Message))
        {
            _ = result.AppendLine(Message);
        }
        if (string.IsNullOrEmpty(Message) && Errors.Count == 1)
        {
            _ = result.AppendLine(Errors[0].Message?.ToString() ?? "An error occurred.");
        }
        else
        {
            foreach (var errorMessage in Errors.Select(x => x.Message?.ToString()))
            {
                _ = result.AppendLine($"- {errorMessage}");
            }
        }

        return result.ToString();
    }

    internal static TResult From<TResult>(in ResultBase source, in TResult dest)
        where TResult : ResultBase
    {
        dest.Status = source.Status;
        dest._message = source.Message;

        dest.Errors.AddRange(source.Errors);
        foreach (var item in source.Extra)
        {
            dest.Extra.Add(item.Key, item.Value);
        }
        return dest;
    }

    internal void SetIsSucceed(bool? isSucceed)
            => _isSucceed = isSucceed;

    private string GetDebuggerDisplay()
            => ToString();
}