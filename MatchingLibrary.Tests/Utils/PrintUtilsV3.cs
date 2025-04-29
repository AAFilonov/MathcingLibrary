using System.Collections.Generic;
using System.Text;
using MatchingLibrary.v3.exampleClasses;

namespace MatchingLibrary.Tests.Utils;

public class PrintUtilsV3
{
    public static string ToString(List<(Tutor, List<Student>)> tutors)
    {
        var sb = new StringBuilder();
        tutors.ForEach(pair => sb.Append(ToString(pair)));
        return sb.ToString();
    }

    private static string ToString((Tutor, List<Student>) tuple)
    {
        var sb = new StringBuilder();

        if (tuple.Item2 != null) tuple.Item2.ForEach(student => sb.Append(student));
        return $"{tuple.Item1}:{sb}, ";
    }
}