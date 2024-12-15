using HsManCommonLibrary.ErrorHandling;

namespace HsManCommonLibrary.Common;

public interface IResult<out TResult, out TError> where TError: IError
{
    bool Success { get; }
    TError? Error { get; }
    TResult? Value { get; }
}