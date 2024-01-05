using MatchingLibrary.v1.Allocation.interfaces.Smp;

namespace MatchingLibrary.v1.Algorithms.interfaces;

public interface IOneToOneGenericAllocationAlgorithm<T,U>
    where T : class
    where U : class
{
    public void computeIteration(ISmpAllocationGeneric<T, U?> allocationGeneric);
    public Boolean isFinal(ISmpAllocationGeneric<T, U?> allocationGeneric);
}