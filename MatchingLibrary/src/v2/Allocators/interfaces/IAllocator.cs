namespace MatchingLibrary.v2.Allocators.interfaces;

public interface IAllocator<in TAllocation>
{
    public void computeIteration(TAllocation allocation);
    public bool isFinal(TAllocation allocation);
}