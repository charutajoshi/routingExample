using Microsoft.AspNetCore.Http; 

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (context, next) =>
{
    Endpoint endpoint = context.GetEndpoint();
    if (endpoint != null)
    {
        await context.Response.WriteAsync($"Endpoint is {endpoint.DisplayName}\n");
    }
    await next(context);
}); 

// Enable routing
app.UseRouting();

app.Use(async (context, next) =>
{
    // Endpoint? = nullable reference type
    Endpoint? endpoint = context.GetEndpoint();

    if (endpoint != null)
    {
        await context.Response.WriteAsync($"Endpoint is {endpoint.DisplayName}\n");
    }

    await next(context);
});

// Create endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("test1", async (context) =>
    {
        await context.Response.WriteAsync("hello1"); 
    });

    endpoints.MapPost("test2", async (context) =>
    {
        await context.Response.WriteAsync("hello2");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"Request received - {context.Request.Path}");
}); 

app.Run();

