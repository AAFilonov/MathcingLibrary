namespace MatchingLibrary.v2.Algorithms;

public interface IAllocator<in TAllocation>
{
    public void computeIteration(TAllocation allocation);
    public bool isFinal(TAllocation allocation);
}