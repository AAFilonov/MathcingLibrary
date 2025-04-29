using System;
using System.Collections.Generic;
using MatchingLibrary.v1.Allocated.Impl;

namespace MatchingLibrary.v1.Allocation.interfaces.atomic;

public interface AllocationH<H>
{
    public void SetCapacityH(int c);
    public int GetCapacityP(H h);
    public List<H> GetPreferenceH();
    public void SetPreferenceH(List<H> prefence);
}
//объект на который ссылаются
public interface AllocationL<H, P> :AllocationH<H>
{
    public List<P> GetListP(H h);
    public H GetHByP(P p);
    public void SetCapacityP(int c);
    public int GetCapacityP(P p);
}

public interface IAllocationTwoStepOtM<S,L,P> : 
    AllocationL<L,P>,
    OneToAllocation<S,P>,
    Assigned<S,P>
{
}

public class AllocationTwoStepOtM<S,L,P>: IAllocationTwoStepOtM<S,L,P>
{
    public void SetCapacityH(int c)
    {
        throw new NotImplementedException();
    }

    public int GetCapacityP(L h)
    {
        throw new NotImplementedException();
    }

    public List<L> GetPreferenceH()
    {
        throw new NotImplementedException();
    }

    public void SetPreferenceH(List<L> prefence)
    {
        throw new NotImplementedException();
    }

    public List<P> GetListP(L h)
    {
        throw new NotImplementedException();
    }

    public L GetHByP(P p)
    {
        throw new NotImplementedException();
    }

    public void SetCapacityP(int c)
    {
        throw new NotImplementedException();
    }

    public int GetCapacityP(P p)
    {
        throw new NotImplementedException();
    }

    public List<S> GetPreferenceM(P w)
    {
        throw new NotImplementedException();
    }

    public void SetPreferenceM(List<S> prefence)
    {
        throw new NotImplementedException();
    }

    public void Assign(S m, P w)
    {
        throw new NotImplementedException();
    }

    public void BreakAssigment(S m, P w)
    {
        throw new NotImplementedException();
    }
}
