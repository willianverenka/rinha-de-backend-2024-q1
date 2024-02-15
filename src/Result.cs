namespace rinha_de_backend_2024_q1;

public readonly struct Result<TValue, TError> where TError : Exception
{
    private readonly TValue? _value;
    private readonly TError? _error;

    public Result(TValue value)
    {
        IsError = false;
        _value = value;
        _error = default;
    }

    public Result(TError error)
    {
        IsError = true;
        _error = error;
        _value = default;
    }
    public bool IsError { get; }
    public bool IsSuccess => !IsError;

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);
    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    public TResult Match<TResult>(
        Func<TValue, TResult> success,
        Func<TError, TResult> failure) =>
        !IsError ? success(_value!) : failure(_error!);
}