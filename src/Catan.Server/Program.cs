using System.Text.Json;
using Catan.Game;
using Catan.GameModes;
using Catan.Modes.Mini;
using Catan.Seafarers;
using Catan.Server;
using Catan.Standard;

var builder = WebApplication.CreateBuilder(args);

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")))
{
    var cloudPort = Environment.GetEnvironmentVariable("PORT");
    if (!string.IsNullOrEmpty(cloudPort))
        builder.WebHost.UseUrls($"http://0.0.0.0:{cloudPort}");
    else
        builder.WebHost.UseUrls($"http://localhost:{builder.Configuration["port"] ?? "5200"}");
}

builder.Services
    .AddSignalR()
    .AddJsonProtocol(options => options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

builder.Services.AddSingleton<GameRegistry>();
builder.Services.AddSingleton(_ =>
{
    IEnumerable<IGameMode> builtIns =
        new IExpansionPack[] { new StandardPack(), new SeafarersPack(), new MiniPack() }
            .SelectMany(pack => pack.Modes);
    return new ModeCatalog(Path.Combine(AppContext.BaseDirectory, "modes"), builtIns);
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapHub<GameHub>("/hub");

app.Run();

public partial class Program;
