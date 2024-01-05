using MatchingLibrary.v1.Allocation.interfaces;

namespace MatchingLibrary.v1.Algorithms.interfaces;

public interface ITwoStepAllocationAlgorithm  <S,L,P>
    where S : class
    where L : class
    where P : class
{
    void  computeIteration(ITwoStepAllocation<S, L, P> allocation);
    bool isFinal(ITwoStepAllocation<S, L, P> allocation);
}