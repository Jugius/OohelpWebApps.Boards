using OohelpWebApps.Boards.Contracts.Common.Enums;

namespace OohelpWebApps.Boards.Contracts;
public abstract class Response
{
    public ResponseStatus Status { get; set; }


    public static Response Ok { get; } = new InternalResponse { Status = ResponseStatus.Ok };
    public static Response InvalidRequest { get; } = new InternalResponse { Status = ResponseStatus.InvalidRequest };


    private class InternalResponse : Response
    {
    }

}
