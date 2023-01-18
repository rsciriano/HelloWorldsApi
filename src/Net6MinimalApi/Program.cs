var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/api/hello", (string name) => $"Hello { name ?? "world" }!");
app.Run();
