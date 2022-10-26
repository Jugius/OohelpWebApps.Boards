using OohelpWebApps.Boards.Contracts.Responses;
using OohelpWebApps.Boards.Exceptions;

namespace OohelpWebApps.Boards.Mapping;
public static class ExceptionToResponse
{
    public static ErrorResponse ToResponse(this Exception exception)
    {
        Contracts.Common.Enums.ResponseStatus status = exception switch
        {
            ApiException a => a.Status,
            _ => Contracts.Common.Enums.ResponseStatus.UnknownError
        };
        return new ErrorResponse(status, exception.Message);
    }
}
