using MatchingLibrary.v2.Allocated;

namespace MatchingLibrary.v2.Allocation;

//allocation for classic Stable Marriage Problem (1 to 1) 
public interface IOneToOneAllocation
{
    List<IToOneAllocated> GetMen();
    List<IToOneAllocated> GetWomen();

    public List<(IAllocated?, IAllocated?)> GetAllocationResult();
}