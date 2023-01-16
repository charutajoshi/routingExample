using System.Net;
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
    endpoints.Map("files/{filename}.{extension}", async context =>
    {
        // string? = nullable reference type
        // allows null values
        string? fileName = Convert.ToString(context.Request.RouteValues["filename"]);
        string? extension = Convert.ToString(context.Request.RouteValues["extension"]);
        await context.Response.WriteAsync($"File name is {fileName}, extension is {extension}");
    });

    endpoints.Map("employee/profile/{EmployeeName}", async context =>
    {
        string? employeeName = Convert.ToString(context.Request.RouteValues["EmployeeName"]);
        await context.Response.WriteAsync($"Employee name is {employeeName}");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"Request received - {context.Request.Path}");
}); 

app.Run();

