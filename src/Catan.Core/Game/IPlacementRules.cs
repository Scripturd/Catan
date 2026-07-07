using Catan.Players;

namespace Catan.Game
{
    public interface IPlacementRules
    {
        bool VertexIsVacant(Vertex vertex);

        bool SatisfiesDistanceRule(Vertex vertex);

        bool TouchesOwnRoad(PlayerId player, Vertex vertex);

        bool EdgeIsVacant(Edge edge);

        bool TouchesOwnBuilding(PlayerId player, Edge edge);

        bool ConnectsToNetwork(PlayerId player, Edge edge);
    }
}