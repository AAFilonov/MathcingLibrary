namespace MatchingLibrary.v2_OLD.Allocators.interfaces;

public interface IAllocator<in TAllocation>
{
    public void computeIteration(TAllocation allocation);
    public bool isFinal(TAllocation allocation);
}