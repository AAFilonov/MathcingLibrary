using MatchingLibrary.v1.Allocated.Impl;

namespace MatchingLibrary.v1.Allocation.interfaces.atomic;

//объект на который ссылаются


public interface IAllocationOtM<H,R> : 
    AllocationH<H>,
    OneToAllocation<H,R>,
    Assigned<H,R>
{
}

public class AllocationOtM: IAllocationOtM<NamedAllocated, NamedAllocated>
{
    public void SetCapacity<H>(int c)
    {
        throw new NotImplementedException();
    }

    public int GetCapacity<H>(H h)
    {
        throw new NotImplementedException();
    }

    public void SetCapacityH(int c)
    {
        throw new NotImplementedException();
    }

    public int GetCapacityP(NamedAllocated h)
    {
        throw new NotImplementedException();
    }

    public List<NamedAllocated> GetPreferenceH()
    {
        throw new NotImplementedException();
    }

    public void SetPreferenceH(List<NamedAllocated> prefence)
    {
        throw new NotImplementedException();
    }

    public List<NamedAllocated> GetPreferenceM()
    {
        throw new NotImplementedException();
    }

    public List<NamedAllocated> GetPreferenceM(NamedAllocated w)
    {
        throw new NotImplementedException();
    }

    public void SetPreferenceM(List<NamedAllocated> prefence)
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