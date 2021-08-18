namespace Searchify.Domain.Utils
{
    public class Response<T>
    {
        public Response(T response, string responseMessage )
        {
            data = response;
            message = responseMessage;
        }
        public T data { get; }
        public string message { get; }
    }
}