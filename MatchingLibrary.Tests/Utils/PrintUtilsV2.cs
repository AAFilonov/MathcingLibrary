using System.Text;
using MatchingLibrary.Utils;
using MatchingLibrary.v1.Allocated.Impl;
using MatchingLibrary.v2.Allocated;

namespace MatchingLibrary.Tests.Utils;

public class PrintUtilsV2
{
    public static string toString((IOneToOneAllocated?, IOneToOneAllocated?) tuple)
    {
        return $"pair: [{tuple.Item1}:{tuple.Item2}],";
    }
    
    public static string toString(ValueTuple<IOneToOneAllocated, List<IOneToOneAllocated>> tuple)
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

    public static string toString(KeyValuePair<IOneToOneAllocated, IOneToOneAllocated> pair)
    {
        return $"pair: [{pair.Key}: {pair.Value}], ";
    }

    public static string toString(Dictionary<IOneToOneAllocated, IOneToOneAllocated> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(toString(pair)));
        return result.ToString();
    }

    public static string toString2(List<(IOneToOneAllocated?, IOneToOneAllocated?)> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(toString(pair)));
        return result.ToString();
    }

    public static string toString(List<(IOneToOneAllocated, List<IOneToOneAllocated>)> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(toString(pair)));
        return result.ToString();
    }

    
    public static object toString(List<(IOneToOneAllocated, List<(IOneToOneAllocated, List<IOneToOneAllocated>)>)> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(toString(pair)));
        return result.ToString();
    }

    private static string toString((IOneToOneAllocated, List<(IOneToOneAllocated, List<IOneToOneAllocated>)>) tuple)
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