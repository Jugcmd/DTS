using Microsoft.Azure.Functions.Worker;

using Microsoft.Azure.Functions.Worker.Http;

using Microsoft.Azure.Functions.Worker.Extensions.Sql;


namespace Company.Function
{
  public class HttpTrigger1
  {
    [Function("HttpTrigger1")]
    public static HttpResponseData Run(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "test")]
      HttpRequestData req,
      [SqlInput(
        "SELECT * FROM [dbo].[Table1]",
        "SQL_CONNECTION_STRING"
      )] IEnumerable<Table1> result)
    {
      return Response.OK(req, result);
    }
  }
}
