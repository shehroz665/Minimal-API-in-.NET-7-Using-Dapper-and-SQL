namespace Web.Api.Models
{
    public class Response<T>
    {
        public int? statusCode { get; set; }
        public string? message { get; set; }
        public T data { get; set; }


        public static Response<T> Success(int code, string msg, T data)
        {
            return new Response<T> { statusCode = code, message = msg, data=data };
        }

        public static Response<T> Error(int code, string msg = "")
        {
            return new Response<T> { statusCode = code, message = msg };
        }
    }
}
