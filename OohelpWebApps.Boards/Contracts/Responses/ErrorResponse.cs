using OohelpWebApps.Boards.Contracts.Common.Enums;

namespace OohelpWebApps.Boards.Contracts.Responses;
public class ErrorResponse : Response
{
    public ErrorResponse() { }
    public ErrorResponse(ResponseStatus status)
    {
        Status = status;
    }
    public ErrorResponse(ResponseStatus status, string error)
    {
        Status = status;
        ErrorMessage = string.IsNullOrEmpty(error) ? null : error;
    }
    public string ErrorMessage { get; set; }
}
