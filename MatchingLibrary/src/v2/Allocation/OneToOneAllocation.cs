using System;
using System.Collections.Generic;
using System.Linq;
using MatchingLibrary.v2.Allocated;

namespace MatchingLibrary.v2.Allocation;

public class OneToOneAllocation : IOneToOneAllocation
{
    public OneToOneAllocation(List<IToOneAllocated> men, List<IToOneAllocated> women)
    {
        this.men = men;
        this.women = women;
    }

    public List<IToOneAllocated> men { get; set; }
    public List<IToOneAllocated> women { get; set; }

    public List<IToOneAllocated> GetMen()
    {
        return men;
    }

    public List<IToOneAllocated> GetWomen()
    {
        return women;
    }

    public List<(IAllocated?, IAllocated?)> GetAllocationResult()
    {
        var result = new List<(IAllocated?, IAllocated?)>();
        result.AddRange(men
            .Select(man => new ValueTuple<IAllocated?, IAllocated?>(man, man.GetAssigned() as IToOneAllocated))
            .ToList());
        result.AddRange(women.Where(w => w.GetAssigned() == null)
            .Select(w => new ValueTuple<IAllocated?, IAllocated?>(null, w)).ToList());
        return result;
    }
}