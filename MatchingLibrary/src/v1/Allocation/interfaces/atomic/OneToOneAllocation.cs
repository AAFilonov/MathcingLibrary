using MatchingLibrary.v1.Allocated.Impl;

namespace MatchingLibrary.v1.Allocation.interfaces.atomic;

//объект на который ссылаются
public interface OneToAllocation<TMan, TWoman>
{
    public List<TMan> GetPreferenceM( TWoman w);
    public void SetPreferenceM(List<TMan> prefence);
}
//объект который ссылается
public interface ToOneAllocation<TMan,TWoman>
{
    public List<TWoman> GetPreferenceW(TMan  m);
    public void SetPreferenceW(List<TWoman> prefence);
}

public interface Assigned<TMan,TWoman>
{
    public void Assign(TMan m, TWoman w);
    public void BreakAssigment(TMan m, TWoman w);
}

public interface IAllocation<M,W> : 
    OneToAllocation<M,W>,
    ToOneAllocation<M,W>,
    Assigned<M,W>
{
}

public class Allocation : IAllocation<NamedAllocated, NamedAllocated>
{
    public List<NamedAllocated> GetPreferenceM(NamedAllocated w)
    {
        throw new NotImplementedException();
    }

    public void SetPreferenceM(List<NamedAllocated> prefence)
    {
        throw new NotImplementedException();
    }

    public List<NamedAllocated> GetPreferenceW(NamedAllocated m)
    {
        throw new NotImplementedException();
    }

    public void SetPreferenceW(List<NamedAllocated> prefence)
    {
        throw new NotImplementedException();
    }

    public void Assign(NamedAllocated m, NamedAllocated w)
    {
        throw new NotImplementedException();
    }

    public void BreakAssigment(NamedAllocated m, NamedAllocated w)
    {
        throw new NotImplementedException();
    }
}