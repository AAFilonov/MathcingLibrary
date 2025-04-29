using System.Collections.Generic;
using MatchingLibrary.v2_OLD.Allocated;

namespace MatchingLibrary.v2_OLD.Allocation;

//allocation for classic Stable Marriage Problem
public interface IOneToManyAllocation
{
    List<IToOneAllocated> GetResidents();
    List<IToManyAllocated> GetHospitals();
    public List<(IAllocated?, List<IAllocated>)> GetAllocationResult();
}