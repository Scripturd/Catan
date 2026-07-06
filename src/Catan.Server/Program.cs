using System.Text.Json;
using Catan.Server;

var builder = WebApplication.CreateBuilder(args);

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")))
{
    var port = builder.Configuration["port"] ?? "5200";
    builder.WebHost.UseUrls($"http://localhost:{port}");
}

builder.Services
    .AddSignalR()
    .AddJsonProtocol(options => options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

builder.Services.AddSingleton<GameRegistry>();
builder.Services.AddSingleton(_ => new ModeCatalog(Path.Combine(AppContext.BaseDirectory, "modes")));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapHub<GameHub>("/hub");

app.Run();

public partial class Program;
