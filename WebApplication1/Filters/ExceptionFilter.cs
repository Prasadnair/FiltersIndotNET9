namespace WebApplication1.Filters
{
    public class ExceptionFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            try
            {
                return await next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return Results.Problem("Something went wrong, please try again.");
            }
        }
    }
}
