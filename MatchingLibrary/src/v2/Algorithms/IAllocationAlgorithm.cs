namespace MatchingLibrary.v2.Algorithms;

public interface IAllocationAlgorithm<in TAllocation>
{
    public void computeIteration(TAllocation allocation);
    public bool isFinal(TAllocation allocation);
}