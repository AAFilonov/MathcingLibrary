using System.Text;
using MatchingLibrary.v1.Allocated.Impl;
using MatchingLibrary.v2.Allocated.impl;

namespace MatchingLibrary.Tests.Utils;

public class PrintUtils
{
    public static string ToString((NamedAllocated, NamedAllocated?) tuple)
    {
        return $"pair: [{tuple.Item1}:{tuple.Item2}],";
    }

    public static string ToString((ComplexToOneAllocated, ComplexToOneAllocated) tuple)
    {
        return $"pair: [{tuple.Item1}:{tuple.Item2}],";
    }


    public static string ToString(ValueTuple<NamedAllocated, List<NamedAllocated>> tuple)
    {
        var sb = new StringBuilder();
        sb.Append("{ ");
        if (tuple.Item2 != null) tuple.Item2.ForEach(student => sb.Append(student + " "));
        sb.Append("}");
        return $"pair: [{tuple.Item1} {sb}], ";
    }

    public static string ToString(KeyValuePair<NamedAllocated, NamedAllocated> pair)
    {
        return $"pair: [{pair.Key}: {pair.Value}], ";
    }

    public static string ToString(Dictionary<NamedAllocated, NamedAllocated> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(ToString(pair)));
        return result.ToString();
    }

    public static string ToString(List<(NamedAllocated, NamedAllocated?)> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(ToString(pair)));
        return result.ToString();
    }

    public static string ToString(List<(NamedAllocated, List<NamedAllocated>)> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(ToString(pair)));
        return result.ToString();
    }


    public static object ToString(List<(NamedAllocated, List<(NamedAllocated, List<NamedAllocated>)>)> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(ToString(pair)));
        return result.ToString();
    }

    private static string ToString((NamedAllocated, List<(NamedAllocated, List<NamedAllocated>)>) tuple)
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