using MatchingLibrary.Allocated;

namespace MatchingLibrary.Allocation;

public class OneToManyFloatAllocation : IOneToManyFloatAllocation
{
    private readonly List<IToManyFloatAllocated> _hospitals;
    private readonly List<IToOneAllocated> _residents;

    public OneToManyFloatAllocation(List<IToManyFloatAllocated> hospitals, List<IToOneAllocated> residents)
    {
        _hospitals = hospitals;
        _residents = residents;
    }

    public List<IToOneAllocated> GetResidents()
    {
        return _residents;
    }

    public List<IToManyFloatAllocated> GetHospitals()
    {
        return _hospitals;
    }

    public List<(IAllocated?, List<IAllocated>)> GetAllocationResult()
    {
        var result = new List<(IAllocated?, List<IAllocated>)>();
        result.AddRange(_hospitals.Select(h =>
            new ValueTuple<IAllocated?, List<IAllocated>>(h, h.GetAssigned())));
        result.AddRange(_residents.Where(r => r.GetAssigned() == null)
            .Select(r => new ValueTuple<IAllocated?, List<IAllocated>>
                (null, new List<IAllocated> { r })
            ));
        return result;
    }
}