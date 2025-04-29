using MatchingLibrary.Allocated;

namespace MatchingLibrary.Allocation;

//Двухэтапное распределение в котором студенты выбирают не самих преподавателей а их проекты
public interface ITwoStepAllocation
{
    List<IToOneAllocated> GetStudents();
    List<ITwoStepAllocated> GetLecturers();
    List<IDependentAllocated> GetProjects();
}