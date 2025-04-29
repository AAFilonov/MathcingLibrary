using MatchingLibrary.v2.Allocation;

namespace MatchingLibrary.v2.Allocated;

public interface ITwoStepAllocated : IToManyAllocated
{
    public List<IDependentAllocated> GetProjects();
    public void SetProjects(List<IDependentAllocated> newProjects);
    public void InitPreferences(ITwoStepAllocation allocation);
    
}