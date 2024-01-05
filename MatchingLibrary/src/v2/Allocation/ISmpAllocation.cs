

using MatchingLibrary.v2.Allocated;

namespace MatchingLibrary.v2.Allocation;

//allocation for classic Stable Marriage Problem
public interface ISmpAllocation
{
    List<IOneToOneAllocated> getMen();
    List<IOneToOneAllocated> getWoman();
}