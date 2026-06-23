using Catan.Game;
using Catan.Game.UseCases;
using Catan.Pieces;

namespace Catan.Cli;

public sealed class CompositionRoot
{
    public HexGrid Grid { get; }
    public TerrainLayout Terrain { get; }
    public NumberLayout Numbers { get; }

    public SettlementRegistry Settlements { get; }
    public CityRegistry Cities { get; }
    public ResourceRegistry Resources { get; }
    public Robber Robber { get; }

    public ProduceResourcesUseCase ProduceResources { get; }
    public BuildSettlementUseCase BuildSettlement { get; }

    public CompositionRoot()
    {
        var (grid, terrain, numbers) = StandardBoard.Create();
        Grid = grid;
        Terrain = terrain;
        Numbers = numbers;

        Settlements = new SettlementRegistry();
        Cities = new CityRegistry();
        Resources = new ResourceRegistry();
        Robber = new Robber(terrain.HexesOf(TerrainKind.Desert).First());

        ProduceResources = new ProduceResourcesUseCase(grid, numbers, terrain, Settlements, Cities, Resources, Robber);
        BuildSettlement = new BuildSettlementUseCase(Settlements, Cities, Resources, Grid);
    }
}