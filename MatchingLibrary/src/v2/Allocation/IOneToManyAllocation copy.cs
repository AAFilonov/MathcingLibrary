using MatchingLibrary.v2.Allocated;

namespace MatchingLibrary.v2.Allocation;

//allocation for float modification of Hospital Residents Problem
public interface IOneToManyFloatAllocation
{
    List<IToOneAllocated> GetResidents();
    List<IToManyFloatAllocated> GetHospitals();
    public List<(IAllocated?, List<IAllocated>)> GetAllocationResult();
}