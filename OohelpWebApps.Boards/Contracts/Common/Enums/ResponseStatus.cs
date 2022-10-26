
namespace OohelpWebApps.Boards.Contracts.Common.Enums;
public enum ResponseStatus
{
    Ok,

    RequestDenied,
    InvalidKey,

    InvalidRequest,

    NotFound,
    DatabaseError,
    FileSystemError,

    UnknownError,
}
