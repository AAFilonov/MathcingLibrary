using System.Text;
using MatchingLibrary.Utils;
using MatchingLibrary.v1.Allocated.Impl;
using MatchingLibrary.v2.Allocated;

namespace MatchingLibrary.Tests.Utils;

public class PrintUtilsV2
{
    public static string toString((IAllocated?, IAllocated?) tuple)
    {
        return $"[{tuple.Item1}:{tuple.Item2}],";
    }
    
    public static string toString(ValueTuple<IAllocated, List<IAllocated>> tuple)
    {
        StringBuilder sb = new StringBuilder();
     
        if (tuple.Item2 != null)
        {
            tuple.Item2.ForEach(student => sb.Append(student));
        }
        return $"[{tuple.Item1}:{sb.ToString()}], ";
    }

    public static string toString(KeyValuePair<IAllocated, IAllocated> pair)
    {
        return $"[{pair.Key}: {pair.Value}], ";
    }

    public static string toString(Dictionary<IAllocated, IAllocated> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(toString(pair)));
        return result.ToString();
    }

    public static string toString2(List<(IAllocated?, IAllocated?)> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(toString(pair)));
        return result.ToString();
    }

    public static string toString(List<(IAllocated, List<IAllocated>)> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(toString(pair)));
        return result.ToString();
    }

    
    public static object toString(List<(IAllocated, List<(IAllocated, List<IAllocated>)>)> pairs)
    {
        var result = new StringBuilder();
        pairs.ToList().ForEach(pair => result.Append(toString(pair)));
        return result.ToString();
    }

    private static string toString((IAllocated, List<(IAllocated, List<IAllocated>)>) tuple)
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