namespace MatchingLibrary.v2.Allocated;

public interface ITwoStepAllocated : IToManyAllocated
{
    public List<IAllocated> GetProjects();
}