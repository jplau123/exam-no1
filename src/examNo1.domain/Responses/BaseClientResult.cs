using System.Net;

namespace examNo1.domain.Responses;

public class BaseClientResult<T> where T : class
{
    public HttpStatusCode StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
    public T? Data { get; set; }
}
