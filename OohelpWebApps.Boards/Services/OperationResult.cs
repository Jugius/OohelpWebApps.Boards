using Microsoft.AspNetCore.Mvc.Formatters;

namespace OohelpWebApps.Boards.Services;
public class OperationResult<T>
{
    public T Value { get; }
    public Exception Error { get; }
    public bool Success { get; }

    private OperationResult(T value)
    {
        Value = value;
        Success = true;
    }
    private OperationResult(Exception error)
    {
        Error = error;
        Success = false;
    }
    public static OperationResult<T> FromResult(T result) => new OperationResult<T>(result);
    public static OperationResult<T> FromError(Exception error) => new OperationResult<T>(error);
    public static OperationResult<T> FromError(string errorMessage) => FromError(new Exception(errorMessage));
}
