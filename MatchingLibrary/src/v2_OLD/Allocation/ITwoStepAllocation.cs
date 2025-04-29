using MatchingLibrary.v2_OLD.Allocated;

namespace MatchingLibrary.v2_OLD.Allocation;

//Двухэтапное распределение в котором студенты выбирают не самих преподавателей а их проекты
public interface ITwoStepAllocation
{
    List<IToOneAllocated> GetStudents();
    List<ITwoStepAllocated> GetLecturers();
    List<IDependentAllocated> GetProjects();
    public List<(IAllocated?, List<IAllocated>)> GetAllocationResult();
}