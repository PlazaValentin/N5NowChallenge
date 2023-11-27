namespace N5NowChallenge.Application.Queries.Base;

public record QueryResponse<T, TResult> where TResult : QueryResponse<T, TResult>, new()
{
    public static TResult Success(T data)
    {
        return new TResult
        {
            Status = "OK",
            Code = 200,
            Result = data
        };
    }

    public static TResult NotFound()
    {
        return new TResult
        {
            Status = "NOTFOUND",
            Code = 404,
        };
    }

    public static TResult Error()
    {
        return new TResult
        {
            Status = "ERROR",
            Code = 400
        };
    }

    public QueryResponse() { }

    public string Status { get; private init; } = "OK";
    public int Code { get; private init; } = 200;
    public T? Result { get; private init; }
}
