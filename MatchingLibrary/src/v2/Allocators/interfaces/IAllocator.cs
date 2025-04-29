namespace MatchingLibrary.v2.Allocators.interfaces;

public interface IAllocator<in TAllocation>
{
    public void computeAllocation(TAllocation allocation)
    {
        while (!isFinal(allocation))
            computeIteration(allocation);
    }
    public void computeIteration(TAllocation allocation);
    public bool isFinal(TAllocation allocation);
}