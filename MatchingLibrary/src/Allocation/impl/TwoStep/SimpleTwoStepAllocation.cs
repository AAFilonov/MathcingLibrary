using MatchingLibrary.Utils;

namespace MatchingLibrary.Allocation.impl.TwoStep;

public class SimpleTwoStepAllocation  :TwoStepAllocation<SimpleAllocated,SimpleAllocated,SimpleAllocated>
{
    public SimpleTwoStepAllocation(List<SimpleAllocated> students, List<SimpleAllocated> lecturers, List<SimpleAllocated> projects) 
        : base(students, lecturers, projects)
    {
    }
}