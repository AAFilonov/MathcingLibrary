namespace MatchingLibrary.Utils;

public class SetOfPairs<K, V>
    where V : class
    where K : class
{
    //TODO двунаправленная хэш-таблица?
    public List<(K, V)> pairs { get; } = new();

    public V? getByKey(K k)
    {
        var result = pairs.Where(tuple => tuple.Item1.Equals(k)).ToList();
        if (result.Count <= 0)
            return null;
        else
            return result.First().Item2;
    }

    public K? getByValue(V v)
    {
        var result = pairs.Where(tuple => tuple.Item2.Equals(v)).ToList();
        if (result.Count <= 0)
            return null;
        else
            return result.First().Item1;
    }

    public List<V> getManyByKey(K k)
    {
        return pairs
            .FindAll(pair => pair.Item1 == k)
            .Select(tuple => tuple.Item2)
            .ToList();
    }

    public List<K> getManyByValue(V v)
    {
        return pairs
            .FindAll(pair => pair.Item2 == v)
            .Select(tuple => tuple.Item1)
            .ToList();
    }

    public void Add(K t, V u)
    {
        pairs.Add((t, u));
    }

    public void Remove(K t, V u)
    {
        pairs.Remove((t, u));
    }

}