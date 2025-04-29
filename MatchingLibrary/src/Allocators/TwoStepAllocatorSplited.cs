using MatchingLibrary.Allocated;
using MatchingLibrary.Allocation;
using MatchingLibrary.Allocators.interfaces;
using MatchingLibrary.Utils;

namespace MatchingLibrary.Allocators;

// Аналогичен алгоритму из TwoStepAllocator, но итерация разеделена на несколько шагов вызываемых независимо
// В данном случае делается допущение что предпочтения преподавателей неизвестны но являются каким то упорядоченным
// подмножеством полного списка студентов. На каждой итерации они вычисляются заново. Является открытым вопрос могут ли
// предпочтения более поздних итераций противорчеитьболее ранним. Кроме того отличием является поведение в некоторых угловых случаях,
// более подробно об этом в MatchingLibrary.Tests.unitTests.TwoStepSpliterdTests.CornerCaseTests
public class TwoStepAllocatorSplited : ITwoStepAllocatorSplited
{
    public void computeIteration(ITwoStepAllocation allocation)
    {
        StartIteration(allocation);
        InitLecturerPreferences(allocation);
        finishIteration(allocation);
    }

    //Этап 1 студенты назначаются преподавателям
    public void StartIteration(ITwoStepAllocation allocation)
    {
        foreach (var student in allocation.GetStudents())
        {
            if (!haveReachablePair(student)) continue;
            var project = (IDependentAllocated)student.GetPreferences().First();
            AllocationUtils.createAssignment(student, project);
        }
    }

    //Этап 2 - преподователи выполняют манипуляции с препдпочтениями
    public void InitLecturerPreferences(ITwoStepAllocation allocation)
    {
        foreach (var lecturer in allocation.GetLecturers()) lecturer.InitPreferences(allocation);
    }

    //Этап 3 - в соотвествии с предпочтениями мы удяляем неприемлемых студентов
    public void finishIteration(ITwoStepAllocation allocation)
    {
        foreach (var student in allocation.GetStudents())
        {
            var project = (IDependentAllocated?)student.GetAssigned();
            if (!project?.GetOwner()?.GetPreferences().Contains(student) ?? false)
            {
                AllocationUtils.breakAssignment(project, student);
                student.GetPreferences().Remove(project);
            }
        }

        foreach (var lecturer in allocation.GetLecturers())
        {
            var projects = lecturer.GetProjects();
            foreach (var project in projects)
                while (project.GetAssigned().Count > project.GetCapacity())
                {
                    var worstStudent = findWorstStudentByProject(project);
                    AllocationUtils.breakAssignment(project, worstStudent);
                    worstStudent.GetPreferences().Remove(project);
                }

            while (lecturer.GetAssigned().Count > lecturer.GetCapacity())
            {
                var worstStudent = findWorstStudentByLecturer(lecturer);
                var projectWithWorstStudent = (IDependentAllocated)worstStudent.GetAssigned()!;
                AllocationUtils.breakAssignment(projectWithWorstStudent, worstStudent);
                worstStudent.GetPreferences().Remove(projectWithWorstStudent);
            }
        }
    }

    public bool isFinal(ITwoStepAllocation allocation)
    {
        var freeStudents = allocation.GetStudents().Where(haveReachablePair);
        return !freeStudents.Any();
    }

    private IToOneAllocated findWorstStudentByLecturer(ITwoStepAllocated lecturer)
    {
        var lecturerPreferences = new List<IToOneAllocated>(lecturer.GetPreferences().Cast<IToOneAllocated>());
        var assignedStudents = lecturer.GetAssigned();

        bool AllNotAssigned(IToOneAllocated allocated)
        {
            return !assignedStudents.Contains(allocated);
        }

        lecturerPreferences.RemoveAll(AllNotAssigned);
        return lecturerPreferences.Last();
    }

    private static IToOneAllocated findWorstStudentByProject(IDependentAllocated project)
    {
        IToManyAllocated lecturer = project.GetOwner()!;
        var preferences = new List<IToOneAllocated>(lecturer.GetPreferences().Cast<IToOneAllocated>());
        preferences.Reverse(); //sort in ascending order.

        var assignedStudents = project.GetAssigned().Cast<IToOneAllocated>().ToList();
        var worstStudent = assignedStudents.First();
        var worstScore = preferences.Count; //less is worse = хуже
        foreach (var student in assignedStudents)
        {
            var score = preferences.FindIndex(allocated => Equals(allocated, student));
            if (score < worstScore)
            {
                worstStudent = student;
                worstScore = score;
            }
        }

        return worstStudent;
    }

    private static bool haveReachablePair(IToOneAllocated student)
    {
        var havePair = student.GetAssigned() != null;
        var canFindPair = student.GetPreferences().Any();
        return !havePair && canFindPair;
    }
}