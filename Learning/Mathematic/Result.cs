
namespace Learning.Mathematic;

public record Error(string Message);
public record Result<TValue, TError>
{
    public TValue Value { get; set; }
    public TError Error { get; set; }

    public static explicit operator Result<TValue, TError>(TError error)
    {
        return new Result<TValue, TError>() { Error = error };
    }

    public static explicit operator Result<TValue, TError>(TValue value)
    {
        return new Result<TValue, TError>() { Value = value };
    }
}