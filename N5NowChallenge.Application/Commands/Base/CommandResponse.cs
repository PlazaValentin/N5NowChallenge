using System.Net;

namespace N5NowChallenge.Application.Commands.Base;

public record CommandResponse<T>
{
    public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;
    public string? ErrorMessage { get; init; }
    public T? Result { get; init; }
}