
namespace WebApplication1.Filters
{
    public class LoggingFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context, 
            EndpointFilterDelegate next)
        {
            Console.WriteLine($"Incoming: {context.HttpContext.Request.Path}");

            var result = await next(context);

            Console.WriteLine($"Completed: {context.HttpContext.Response.StatusCode}");

            return result;

        }
    }
}
