using Catan.Game;

namespace Catan.Modding;

public sealed class PluginLoader
{
    private readonly Action<string>? _log;

    public PluginLoader(Action<string>? log = null)
    {
        _log = log;
    }

    public IReadOnlyList<GameModeRegistration> Load(string directory)
    {
        if (!Directory.Exists(directory))
            return [];

        var registrations = new List<GameModeRegistration>();
        foreach (var path in Directory.EnumerateFiles(directory, "*.dll").OrderBy(p => p, StringComparer.OrdinalIgnoreCase))
        {
            try
            {
                var assembly = new PluginLoadContext(path).LoadFromAssemblyPath(path);
                var pluginTypes = assembly.GetTypes()
                    .Where(t => typeof(IGameModePlugin).IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false });

                foreach (var type in pluginTypes)
                {
                    if (Activator.CreateInstance(type) is not IGameModePlugin plugin)
                        continue;
                    registrations.AddRange(plugin.Modes);
                    _log?.Invoke($"Loaded plugin '{type.FullName}' from {Path.GetFileName(path)}.");
                }
            }
            catch (Exception ex)
            {
                _log?.Invoke($"Skipped plugin '{Path.GetFileName(path)}': {ex.Message}");
            }
        }

        return registrations;
    }
}
