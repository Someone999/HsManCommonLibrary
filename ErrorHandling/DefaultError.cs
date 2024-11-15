namespace HsManCommonLibrary.ErrorHandling;

public class DefaultError<T> : IError<T>
{
    public string? StackTrace { get; set; }
    public string? Cause { get; set; }
    public ErrorLevel Level { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }

    object? IError.Data => Data;
}