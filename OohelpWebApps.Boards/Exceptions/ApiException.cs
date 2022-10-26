using OohelpWebApps.Boards.Contracts.Common.Enums;

namespace OohelpWebApps.Boards.Exceptions;
public class ApiException : Exception
{
    public ResponseStatus Status { get; }

    public ApiException(ResponseStatus status) : base(string.Empty)
    {
        this.Status = status;
    }
    public ApiException(ResponseStatus status, string message) : base(message)
    {
        this.Status = status;
    }
}
