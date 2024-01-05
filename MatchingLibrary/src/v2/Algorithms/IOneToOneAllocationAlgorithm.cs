using MatchingLibrary.v2.Allocation;

namespace MatchingLibrary.v2.Algorithms;

public interface IOneToOneAllocationAlgorithm
{
    public void computeIteration(ISmpAllocation allocation);
    public Boolean isFinal(ISmpAllocation allocation);
}