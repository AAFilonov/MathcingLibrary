using MatchingLibrary.Allocated;

namespace MatchingLibrary.Allocation;

public class TwoStepAllocation : ITwoStepAllocation
{
    private readonly List<ITwoStepAllocated> _lecturers = new();
    private readonly List<IDependentAllocated> _projects = new();
    private readonly List<IToOneAllocated> _students = new();

    public List<IToOneAllocated> GetStudents()
    {
        return _students;
    }

    public List<ITwoStepAllocated> GetLecturers()
    {
        return _lecturers;
    }

    public List<IDependentAllocated> GetProjects()
    {
        return _projects;
    }
}