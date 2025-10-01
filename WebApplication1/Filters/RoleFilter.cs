namespace WebApplication1.Filters
{
    public class RoleFilter : IEndpointFilter
    {
        private readonly string _role;
        public RoleFilter(string role) => _role = role;

        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? false)
                return Results.Unauthorized();

            if (!user.IsInRole(_role))
                return Results.Forbid();

            return await next(context);
        }
    }
}
