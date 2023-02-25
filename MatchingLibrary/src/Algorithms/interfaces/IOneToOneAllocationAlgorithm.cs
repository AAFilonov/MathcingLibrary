using MatchingLibrary.Allocation.impl;
using MatchingLibrary.Allocation.interfaces;

namespace MatchingLibrary.Algorithms.interfaces;

public interface IOneToOneAllocationAlgorithm<T,U>
    where T : class
    where U : class
{
    public void computeIteration(ISmpAllocation<T,U> allocation);
    public Boolean isFinal(ISmpAllocation<T,U> allocation);
}