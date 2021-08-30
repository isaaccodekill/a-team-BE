namespace Searchify.Domain.Utils
{
    /// <summary>
    /// Class that helps structure json response
    /// </summary>
    /// <typeparam name="T">Type of data in response</typeparam>
    public class Response<T>
    {
        public Response(T response, string responseMessage )
        {
            data = response;
            message = responseMessage;
        }
        /// <summary>
        /// data passed to response
        /// </summary>
        public T data { get; }
        /// <summary>
        /// message sent
        /// </summary>
        public string message { get; }
    }
}