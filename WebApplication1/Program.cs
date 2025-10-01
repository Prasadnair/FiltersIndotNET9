using WebApplication1.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//use inline filter
app.MapGet("/inlinefilters", (string name) => $"Hello, {name}!")
   .AddEndpointFilter(async (context, next) =>
   {
       Console.WriteLine("Before endpoint logic");

       var result = await next(context);

       Console.WriteLine("After endpoint logic");
       return result;
   });

// Using a reusable filter
app.MapGet("/reusefilter", (string name) => $"Hello, {name}!")
   .AddEndpointFilter<WebApplication1.Filters.LoggingFilter>();

// Using a generic validation filter
app.MapPost("/validationfilter", (WebApplication1.Models.UserDto user) => Results.Ok($"User {user.Name} of age {user.Age} created."))
   .AddEndpointFilter<ValidationFilter<WebApplication1.Models.UserDto>>();

//usiing role based filter
app.MapGet("/rolefilter", () => "Welcome, Admin")
   .AddEndpointFilterFactory((factoryContext, next) =>
       (context) =>
       {
           var filter = new RoleFilter("Admin");
           return filter.InvokeAsync(context, next);
       });

// Using a global exception handling filter
var api = app.MapGroup("/api")
      .AddEndpointFilter<ExceptionFilter>();

api.MapGet("/exceptionfilter", () => { throw new Exception("Test exception"); }); 


app.Run();







