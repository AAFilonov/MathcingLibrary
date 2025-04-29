using MatchingLibrary.v2_OLD.Allocated;

namespace MatchingLibrary.v2_OLD.Allocation;

//allocation for classic Stable Marriage Problem (1 to 1) 
public interface IOneToOneAllocation
{
    List<IToOneAllocated> GetMen();
    List<IToOneAllocated> GetWomen();

    public List<(IAllocated?, IAllocated?)> GetAllocationResult();
}