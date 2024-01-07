using System.Text;
using MatchingLibrary.Utils;
using MatchingLibrary.v1.Allocated.Impl;
using MatchingLibrary.v2.Allocated;
using MatchingLibrary.v3;
using MatchingLibrary.v3.exampleClasses;

namespace MatchingLibrary.Tests.Utils;

public class PrintUtilsV3
{
    public static string toString(List<(Tutor, List<Student>)> tutors)
    {
        StringBuilder sb = new StringBuilder();
        tutors.ForEach(pair => sb.Append(toString(pair)));
        return sb.ToString();
    }

    private static string toString((Tutor, List<Student>) tuple)
    {
        StringBuilder sb = new StringBuilder();
     
        if (tuple.Item2 != null)
        {
            tuple.Item2.ForEach(student => sb.Append(student));
        }
        return $"{tuple.Item1}:{sb.ToString()}, ";
    }
}