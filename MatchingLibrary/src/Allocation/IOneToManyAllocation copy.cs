using MatchingLibrary.Allocated;

namespace MatchingLibrary.Allocation;

//allocation for float modification of Hospital Residents Problem
public interface IOneToManyFloatAllocation
{
    List<IToOneFloatAllocated> GetSubordinates();
    List<IToManyFloatAllocated> GetMasters();
    public List<(IAllocated?, List<IAllocated>)> GetAllocationResult();
    void refreshPreferences(IToOneFloatAllocated iToOneFloatAllocated);
}