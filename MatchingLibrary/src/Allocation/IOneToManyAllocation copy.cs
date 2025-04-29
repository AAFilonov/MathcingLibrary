using MatchingLibrary.Allocated;

namespace MatchingLibrary.Allocation;

//allocation for float modification of Hospital Residents Problem
public interface IOneToManyFloatAllocation
{
    List<IToOneAllocated> GetResidents();
    List<IToManyFloatAllocated> GetHospitals();
    public List<(IAllocated?, List<IAllocated>)> GetAllocationResult();
}