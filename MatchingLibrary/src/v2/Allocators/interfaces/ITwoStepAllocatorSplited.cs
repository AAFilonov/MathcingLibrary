using MatchingLibrary.v2.Allocation;

namespace MatchingLibrary.v2.Allocators.interfaces;

public interface ITwoStepAllocatorSplited :IAllocator<ITwoStepAllocation>
{
    public void StartIteration(ITwoStepAllocation allocation);
    public void InitLecturerPreferences(ITwoStepAllocation allocation);
    public void finishIteration(ITwoStepAllocation allocation);
}