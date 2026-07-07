namespace Catan;

public class Shuffler
{
    private readonly Random _random;

    public Shuffler(Random random)
    {
        _random = random;
    }

    public List<T> Shuffle<T>(IReadOnlyList<T> items)
    {
        var pool = items.ToList();
        var shuffled = new List<T>(pool.Count);
        while (pool.Count > 0)
        {
            var index = _random.Next(0, pool.Count);
            shuffled.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return shuffled;
    }
}