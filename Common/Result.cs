using HsManCommonLibrary.ErrorHandling;

namespace HsManCommonLibrary.Common;

public class Result<TResult, TError> : IResult<TResult, TError> where TError : IError
{
    
    public static Result<TResult, TError> CreateSuccess(TResult value)
    {
        return new Result<TResult, TError>()
        {
            Success = true,
            Error = default,
            Value = value
        };
    }
    
    public static Result<TResult, TError> CreateFailed(TError error)
    {
        return new Result<TResult, TError>()
        {
            Success = false,
            Error = error,
            Value = default
        };
    }

    public bool Success { get; private set; }
    public TError? Error { get; private set; }
    public TResult? Value { get; private set; }
}