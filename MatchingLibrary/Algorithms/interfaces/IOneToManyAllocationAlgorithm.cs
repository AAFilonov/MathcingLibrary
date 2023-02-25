using MatchingLibrary.Allocation.interfaces;

namespace MatchingLibrary.Algorithms.interfaces;

public interface IOneToManyAllocationAlgorithm<S,L>
    where S : class
    where L : class
{
    void computeIteration(IOneToManyAllocation<S, L> allocation);
    bool isFinal(IOneToManyAllocation<S, L> allocation);
}