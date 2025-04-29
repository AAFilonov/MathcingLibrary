using MatchingLibrary.Allocation;

namespace MatchingLibrary.Allocated;

public class BaseTwoStepAllocated : BaseToManyAllocated, ITwoStepAllocated
{
    private List<IDependentAllocated> projects = new();

    public BaseTwoStepAllocated(int capacity) : base(capacity)
    {
    }

    public List<IDependentAllocated> GetProjects()
    {
        return projects;
    }

    public void SetProjects(List<IDependentAllocated> newProjects)
    {
        projects = newProjects;
    }

    public void InitPreferences(ITwoStepAllocation allocation)
    {
        //ничего не делать так как для этого распределяемого предпочтения известны заранее
    }

    public List<IAllocated> GetAssigned()
    {
        return projects.SelectMany(allocated => allocated.GetAssigned()).ToList();
    }
}