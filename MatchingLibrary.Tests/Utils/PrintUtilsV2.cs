using System.Text;
using MatchingLibrary.v2.Allocated;

namespace MatchingLibrary.Tests.Utils;

public class PrintUtilsV2
{
    public static string ToString((IAllocated?, IAllocated?) tuple)
    {
        return $"[{tuple.Item1}:{tuple.Item2}],";
    }

    public static string ToString(ValueTuple<IAllocated, List<IAllocated>> tuple)
    {
        var sb = new StringBuilder();

        if (tuple.Item2 != null) tuple.Item2.ForEach(student => sb.Append(student));
        return $"[{tuple.Item1}:{sb}], ";
    }

    public static string ToString(KeyValuePair<IAllocated, IAllocated> pair)
    {
        return $"[{pair.Key}: {pair.Value}], ";
    }

    public static string ToString(Dictionary<IAllocated, IAllocated> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(ToString(pair)));
        return result.ToString();
    }

    public static string ToString2(List<(IAllocated?, IAllocated?)> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(ToString(pair)));
        return result.ToString();
    }

    public static string ToString(List<(IAllocated, List<IAllocated>)> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(ToString(pair)));
        return result.ToString();
    }


    public static object ToString(List<(IAllocated, List<(IAllocated, List<IAllocated>)>)> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(ToString(pair)));
        return result.ToString();
    }

    private static string ToString((IAllocated, List<(IAllocated, List<IAllocated>)>) tuple)
    {
        var sb = new StringBuilder();
        sb.Append("{ ");
        if (tuple.Item2 != null)
            tuple.Item2.ForEach(projectTuple =>
            {
                sb.Append(projectTuple.Item1);
                sb.Append(" ( ");
                projectTuple.Item2.ForEach(student => sb.Append(student + " "));
                sb.Append(")");
            });
        sb.Append("}");
        return $"pair: [{tuple.Item1} {sb}], ";
    }
}