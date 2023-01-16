var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

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

