using GameSearchApi;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapHttpRoutes();

app.Run();
