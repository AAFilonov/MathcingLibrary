using MatchingLibrary.v1.Allocation.interfaces;

namespace MatchingLibrary.v1.Algorithms.interfaces;

public interface IOneToManyAllocationAlgorithm<S,L>
    where S : class
    where L : class
{
    void computeIteration(IOneToManyAllocation<S, L> allocation);
    bool isFinal(IOneToManyAllocation<S, L> allocation);
}