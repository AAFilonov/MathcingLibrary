using System.Text;
using MatchingLibrary.Utils;
using MatchingLibrary.v1.Allocated.Impl;
using MatchingLibrary.v2.Allocated;
using MatchingLibrary.v2.Allocated.impl;

namespace MatchingLibrary.Tests.Utils;

public class PrintUtils
{
    public static string toString((NamedAllocated, NamedAllocated?) tuple)
    {
        return $"pair: [{tuple.Item1}:{tuple.Item2}],";
    }
    public static string toString((ComplexToOneAllocated, ComplexToOneAllocated) tuple)
    {
        return $"pair: [{tuple.Item1}:{tuple.Item2}],";
    }


    public static string toString(ValueTuple<NamedAllocated, List<NamedAllocated>> tuple)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{ ");
        if (tuple.Item2 != null)
        {
            tuple.Item2.ForEach(student => sb.Append(student + " "));
        }
        sb.Append("}");
        return $"pair: [{tuple.Item1} {sb.ToString()}], ";
    }

    public static string toString(KeyValuePair<NamedAllocated, NamedAllocated> pair)
    {
        return $"pair: [{pair.Key}: {pair.Value}], ";
    }

    public static string toString(Dictionary<NamedAllocated, NamedAllocated> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(toString(pair)));
        return result.ToString();
    }

    public static string toString(List<(NamedAllocated, NamedAllocated?)> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(toString(pair)));
        return result.ToString();
    }

    public static string toString(List<(NamedAllocated, List<NamedAllocated>)> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(toString(pair)));
        return result.ToString();
    }

    
    public static object toString(List<(NamedAllocated, List<(NamedAllocated, List<NamedAllocated>)>)> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(toString(pair)));
        return result.ToString();
    }

    private static string toString((NamedAllocated, List<(NamedAllocated, List<NamedAllocated>)>) tuple)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{ ");
        if (tuple.Item2 != null)
        {
            tuple.Item2.ForEach(projectTuple =>
            {
                sb.Append(projectTuple.Item1);
                sb.Append(" ( ");
                projectTuple.Item2.ForEach(student =>   sb.Append(student+" "));
                sb.Append(")");

            });
          
        }
        sb.Append("}");
        return $"pair: [{tuple.Item1} {sb.ToString()}], ";
    }
}