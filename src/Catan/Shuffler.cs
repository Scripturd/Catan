namespace Catan;

internal static class Shuffler
{
    public static List<T> Shuffle<T>(IReadOnlyList<T> items, Random random)
    {
        var pool = items.ToList();
        var shuffled = new List<T>(pool.Count);
        while (pool.Count > 0)
        {
            var index = random.Next(0, pool.Count);
            shuffled.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return shuffled;
    }
}
