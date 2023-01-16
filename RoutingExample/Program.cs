using System.Net;
using Microsoft.AspNetCore.Http;
using RoutingExample.CustomConstraints; 

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(options =>
{
    // Register MonthsCustomConstraint custom constraint in the Services 
    options.ConstraintMap.Add("months", typeof(MonthsCustomConstraint));
});
var app = builder.Build();

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

    endpoints.Map("employee/profile/{EmployeeName:length(3,7):alpha=none}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("EmployeeName"))
        {
            string? employeeName = Convert.ToString(context.Request.RouteValues["EmployeeName"]);
            await context.Response.WriteAsync($"Employee name is {employeeName}");
        }
        else
        {
            await context.Response.WriteAsync($"Employee name is not provided");
        }
    });

    endpoints.Map("product/{id:int:range(1,100)?}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            int id = Convert.ToInt32(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Product ID is {id}");
        }
        else
        {
            await context.Response.WriteAsync($"Product ID is not provided");
        }
    });

    endpoints.Map("daily-digest-report/{reportDate:datetime}", async context =>
    {
        var reportDate = Convert.ToDateTime(context.Request.RouteValues["reportDate"]);
        await context.Response.WriteAsync($"Report date is {reportDate.ToShortDateString()}");
    });

    endpoints.Map("sales-report/{year:int:min(1900)}/{month:months}", async context =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);
        await context.Response.WriteAsync($"Sales report as of {month} {year}");
    });

    endpoints.Map("sales-report/2023/may", async context =>
    {
        await context.Response.WriteAsync($"Sales report as of May 2023");
    });

    endpoints.Map("cities/{cityID:guid}", async context =>
    {
        var cityID = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityID"]));
        await context.Response.WriteAsync($"City ID is {cityID}");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"{context.Request.Path} does not exist.");
}); 

app.Run();

