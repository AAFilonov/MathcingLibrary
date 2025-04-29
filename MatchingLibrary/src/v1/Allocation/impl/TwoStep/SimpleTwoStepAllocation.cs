using System.Collections.Generic;
using MatchingLibrary.v1.Allocated.Impl;

namespace MatchingLibrary.v1.Allocation.impl.TwoStep;

public class SimpleTwoStepAllocation  :TwoStepAllocation<NamedAllocated,NamedAllocated,NamedAllocated>
{
    public SimpleTwoStepAllocation(List<NamedAllocated> students, List<NamedAllocated> lecturers, List<NamedAllocated> projects) 
        : base(students, lecturers, projects)
    {
    }
}