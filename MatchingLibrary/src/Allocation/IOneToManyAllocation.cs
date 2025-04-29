using MatchingLibrary.Allocated;

namespace MatchingLibrary.Allocation;

//allocation for classic Stable Marriage Problem
public interface IOneToManyAllocation
{
    List<IToOneAllocated> GetResidents();
    List<IToManyAllocated> GetHospitals();
    public List<(IAllocated?, List<IAllocated>)> GetAllocationResult();
}