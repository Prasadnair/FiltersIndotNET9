using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Filters
{
    public class ValidationFilter<UserDto> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var model = context.Arguments.OfType<UserDto>().FirstOrDefault();
            if (model == null)
            {
                return Results.BadRequest("Invalid request payload.");
            }

            // Use reflection to check for UserName, Name, and Age properties
            var type = typeof(UserDto);
            var nameProp = type.GetProperty("Name");
            var ageProp = type.GetProperty("Age");

            var name = nameProp?.GetValue(model) as string;
            var ageValue = ageProp?.GetValue(model);
            int age = 0;
            if (ageValue != null && int.TryParse(ageValue.ToString(), out var parsedAge))
            {
                age = parsedAge;
            }

            if (string.IsNullOrWhiteSpace(name) || age <= 0)
            {
                return Results.BadRequest("Invalid request payload. Name and Age are required and Age must be greater than 0.");
            }

            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(
                model, new ValidationContext(model), validationResults, true);

            if (!isValid)
            {
                var errors = validationResults.ToDictionary(
                    r => r.MemberNames.FirstOrDefault() ?? "Unknown",
                    r => new[] { r.ErrorMessage ?? "Validation error" });

                return Results.ValidationProblem(errors);
            }

            // Await the next delegate in the pipeline
            return await next(context);
        }
    }
}
