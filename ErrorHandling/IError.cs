namespace HsManCommonLibrary.ErrorHandling;

public interface IError
{
    string? StackTrace { get; }
    string? Cause { get; }
    ErrorLevel Level { get; }
    object? Data { get; }
    string? Message { get; }
}

public interface IError<out T> : IError
{
    new T? Data { get; }
}

