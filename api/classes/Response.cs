using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public class Response
    {
        private readonly ILogger _logger;
        private readonly string _action;

        public Response(ILogger logger, string action)
        {
            _logger = logger;
            _action = action.ToLower();
        }

        /* 200 */
        public static HttpResponseData OK<T>(HttpRequestData request, T data)
        {
            HttpResponseData response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteStringAsync(JsonConvert.SerializeObject(data));
            return response;
        }

        /* 204 */
        public static HttpResponseData NoContent(HttpRequestData request)
        {
            return request.CreateResponse(HttpStatusCode.NoContent);
        }

        /* 400 */
        public HttpResponseData BadRequest(HttpRequestData request)
        {
            HttpResponseData response = request.CreateResponse(HttpStatusCode.BadRequest);
            response.WriteStringAsync($"Error {_action}. Data is missing or badly formatted.");
            return response;
        }

        /* 401 */
        public static HttpResponseData Unauthorized(HttpRequestData request, string message = "")
        {
            HttpResponseData response = request.CreateResponse(HttpStatusCode.Unauthorized);
            response.WriteStringAsync(message ?? $"You do not have permission to perform this action.");
            return response;
        }

        /* 404 */
        public static HttpResponseData NotFound<T>(HttpRequestData request, string message = "")
        {
            HttpResponseData response = request.CreateResponse(HttpStatusCode.NotFound);
            response.WriteStringAsync($"{typeof(T).Name} could not be found. {message}");
            return response;
        }

        /* 500 */
        public HttpResponseData Error(HttpRequestData request, Exception e, string message = "")
        {
            HttpResponseData response = request.CreateResponse(HttpStatusCode.InternalServerError);
            response.WriteStringAsync($"Error {_action}. {message}");
            _logger.LogError("{Message}", e.Message);
            return response;
        }
    }
}