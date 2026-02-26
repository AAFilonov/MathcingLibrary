using MatchingLibrary.Allocated;

namespace MatchingLibrary.Allocation;

public class OneToManyFloatAllocation : IOneToManyFloatAllocation
{
    private readonly List<IToManyFloatAllocated> _hospitals;
    private readonly List<IToOneFloatAllocated> _residents;
    private readonly Func<IToOneFloatAllocated,IToManyFloatAllocated, double> _residentPreferenceFunc;

    public OneToManyFloatAllocation(List<IToManyFloatAllocated> hospitals, List<IToOneFloatAllocated> residents, Func<IToOneFloatAllocated, IToManyFloatAllocated, double> residentPreferenceFunc)
    {
        _hospitals = hospitals;
        _residents = residents;
        _residentPreferenceFunc = residentPreferenceFunc;
    }

    public List<IToOneFloatAllocated> GetSubordinates()
    {
        return _residents;
    }
    

    public List<IToManyFloatAllocated> GetMasters()
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

    public void refreshPreferences(IToOneFloatAllocated iToOneFloatAllocated)
    {
        iToOneFloatAllocated.SetPreferences(
            GetMasters().OrderByDescending(m => _residentPreferenceFunc(iToOneFloatAllocated, m))
                .ToList().ConvertAll<IAllocated>(m =>m));
    }
       
}

