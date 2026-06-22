using Catan.Domain.Board;
using NUnit.Framework;

namespace Catan.Domain.Tests.Board;

/// <summary>
/// Proof that the domain is testable with zero UI or engine involvement.
/// Uses the constraint model (Assert.That / Is.EqualTo) so it reads cleanly and
/// stays robust across NUnit versions.
/// </summary>
public class TerrainExtensionsTests
{
    [TestCase(TerrainType.Forest,    ResourceType.Lumber)]
    [TestCase(TerrainType.Hills,     ResourceType.Brick)]
    [TestCase(TerrainType.Pasture,   ResourceType.Wool)]
    [TestCase(TerrainType.Fields,    ResourceType.Grain)]
    [TestCase(TerrainType.Mountains, ResourceType.Ore)]
    public void Resource_terrains_produce_their_resource(TerrainType terrain, ResourceType expected)
    {
        Assert.That(terrain.Produces(), Is.EqualTo(expected));
    }

    [Test]
    public void Desert_produces_nothing()
    {
        Assert.That(TerrainType.Desert.Produces(), Is.Null);
    }
}
