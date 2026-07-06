using Catan.Modes.Mini;
using Catan.Server;

namespace Catan.Tests;

public sealed class PluginLoaderTests
{
    [Fact]
    public void Loads_game_mode_registrations_from_a_plugin_assembly()
    {
        var pluginDll = typeof(MiniPlugin).Assembly.Location;
        var dir = Path.Combine(Path.GetTempPath(), "catan-plugin-test-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        try
        {
            File.Copy(pluginDll, Path.Combine(dir, Path.GetFileName(pluginDll)));

            var modes = new PluginLoader().Load(dir);

            var mini = Assert.Single(modes);
            Assert.Equal("Mini Duel", mini.Name);
            Assert.Equal(2, mini.MinPlayers);
            Assert.Equal(2, mini.MaxPlayers);
        }
        finally
        {
            try { Directory.Delete(dir, recursive: true); } catch (IOException) { } catch (UnauthorizedAccessException) { }
        }
    }

    [Fact]
    public void Returns_empty_for_a_missing_directory()
    {
        Assert.Empty(new PluginLoader().Load(Path.Combine(Path.GetTempPath(), "catan-no-such-plugins")));
    }
}
