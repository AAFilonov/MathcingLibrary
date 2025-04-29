using MatchingLibrary.Allocation;

namespace MatchingLibrary.Allocated;

public interface ITwoStepAllocated : IToManyAllocated
{
    public List<IDependentAllocated> GetProjects();
    public void SetProjects(List<IDependentAllocated> newProjects);
    public void InitPreferences(ITwoStepAllocation allocation);
}