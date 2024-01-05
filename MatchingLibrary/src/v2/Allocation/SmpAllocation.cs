using MatchingLibrary.v2.Allocated;

namespace MatchingLibrary.v2.Allocation;

public class SmpAllocation :ISmpAllocation
{
    public SmpAllocation(List<IOneToOneAllocated> men, List<IOneToOneAllocated> women)
    {
        this.men = men;
        this.women = women;
    }
    public List<IOneToOneAllocated> men { get; set; }
    public List<IOneToOneAllocated> women { get; set; }
    
    public List<IOneToOneAllocated> getMen()
    {
        return men;
    }
    public List<IOneToOneAllocated> getWoman()
    {
        return women;
    }

    public List<(IOneToOneAllocated?, IOneToOneAllocated?)> CalcFinalAllocation()
    {
        var result = new List<(IOneToOneAllocated?, IOneToOneAllocated?)>();
        result.AddRange(men.Select(man => new ValueTuple<IOneToOneAllocated?, IOneToOneAllocated?>(man, man.GetAssigned() as IOneToOneAllocated)).ToList());
        result.AddRange(women.Where(w => w.GetAssigned() == null)
            .Select(w => new ValueTuple<IOneToOneAllocated?, IOneToOneAllocated?>(null, w)).ToList());
        return result;
    }
}