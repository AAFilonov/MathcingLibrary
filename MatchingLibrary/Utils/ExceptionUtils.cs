namespace MatchingLibrary.Utils;

public class ExceptionUtils
{
    public static void checkPresense<T>(T obj, List<T> list)
    {
        if (!list.Contains(obj))
            throw new ArgumentException($"Аргумент {obj} не входит в список распределяемых {list}!");
    }
}