using System.Collections.Generic;
using MatchingLibrary.v2.Allocated;

namespace MatchingLibrary.v2.Allocation;

//allocation for classic Stable Marriage Problem
public interface IOneToManyAllocation
{
    List<IToOneAllocated> GetResidents();
    List<IToManyAllocated> GetHospitals();
    public List<(IAllocated?, List<IAllocated>)> GetAllocationResult();
}