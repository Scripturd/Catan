using Catan.Game;

namespace Catan.GameModes;

public sealed class PluginLoader
{
    private readonly Action<string>? _log;

    public PluginLoader(Action<string>? log = null)
    {
        _log = log;
    }

    public IReadOnlyList<IGameMode> Load(string directory)
    {
        if (!Directory.Exists(directory))
            return [];

        var modes = new List<IGameMode>();
        foreach (var path in Directory.EnumerateFiles(directory, "*.dll").OrderBy(p => p, StringComparer.OrdinalIgnoreCase))
        {
            try
            {
                var assembly = new PluginLoadContext(path).LoadFromAssemblyPath(path);
                var packTypes = assembly.GetTypes()
                    .Where(t => typeof(IExpansionPack).IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false });

                foreach (var type in packTypes)
                {
                    if (Activator.CreateInstance(type) is not IExpansionPack pack)
                        continue;
                    modes.AddRange(pack.Modes);
                    _log?.Invoke($"Loaded expansion pack '{type.FullName}' from {Path.GetFileName(path)}.");
                }
            }
            catch (Exception ex)
            {
                _log?.Invoke($"Skipped plugin '{Path.GetFileName(path)}': {ex.Message}");
            }
        }

        return modes;
    }
}
