using System.Net;
using Newtonsoft.Json;

namespace Sample.Api.Middlewares
{
    public class ResponseWrapper
    {
        private readonly RequestDelegate _next;

        public ResponseWrapper(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var currentBody = context.Response.Body;

            using (var memoryStream = new MemoryStream())
            {
                //set the current response to the memorystream.
                context.Response.Body = memoryStream;

                await _next(context);

                //reset the body 
                context.Response.Body = currentBody;

                if (context.Request.Path.ToUriComponent().StartsWith("/api"))
                {
                    if (context.Response.StatusCode != 204)
                    {
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        var readToEnd = new StreamReader(memoryStream).ReadToEnd();
                        var objResult = JsonConvert.DeserializeObject(readToEnd);
                        var result = CommonApiResponse.Create((HttpStatusCode)context.Response.StatusCode, objResult);
                        context.Response.ContentLength = System.Text.Encoding.UTF8.GetByteCount(JsonConvert.SerializeObject(result));
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                    }
                }
                else
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsync(string.Empty);
                }

            }
        }

    }

    public static class ResponseWrapperExtensions
    {
        public static IApplicationBuilder UseResponseWrapper(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseWrapper>();
        }
    }


    public class CommonApiResponse
    {
        public static CommonApiResponse Create(HttpStatusCode statusCode, object result)
        {
            return new CommonApiResponse(statusCode, result);
        }

        public string message { get; set; }
        public object data { get; set; }
        public string timestamp { get; set; }
        public bool success { get; set; }

        protected CommonApiResponse(HttpStatusCode statusCode, object result)
        {
            timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    data = result;
                    success = true;
                    break;
                case HttpStatusCode.Created:
                    data = result;
                    success = true;
                    break;
                case HttpStatusCode.NoContent:
                    success = true;
                    break;
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.NotFound:
                default:
                    message = JsonConvert.SerializeObject(result, Formatting.None);
                    break;
            }
        }
    }
}
