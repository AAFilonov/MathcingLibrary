using System.Collections.Generic;
using System.Linq;
using MatchingLibrary.Allocation;

namespace MatchingLibrary.Tests.Utils;

public class TestUtilsV2
{
    public static Dictionary<string, List<string>> GetAllocationResult(ITwoStepAllocation allocation)
    {
        var result = new Dictionary<string, List<string>>();
        foreach (var project in allocation.GetProjects())
            result.Add(project.ToString(), project.GetAssigned().Select(s => s.ToString()).ToList());

        var unassignedStudents = allocation.GetStudents().Where(student => student.GetAssigned() == null).ToList();
        result.Add("None", unassignedStudents.Select(allocated => allocated.ToString()).ToList());

        return result;
    }
}