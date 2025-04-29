using MatchingLibrary.Allocated;
using MatchingLibrary.Allocation;
using MatchingLibrary.Allocators.interfaces;
using MatchingLibrary.Utils;

namespace MatchingLibrary.Allocators;

// ориентированный на студентов алгоритм двух этапного распределения
public class TwoStepAllocator : IAllocator<ITwoStepAllocation>
{
    public void computeIteration(ITwoStepAllocation allocation)
    {
        foreach (var student in allocation.GetStudents())
            if (haveReachablePair(student))
                findPair(student);
    }

    public bool isFinal(ITwoStepAllocation allocation)
    {
        var freeStudents = allocation.GetStudents().Where(haveReachablePair);
        return !freeStudents.Any();
    }

    private void findPair(IAllocated student)
    {
        var project = (IDependentAllocated)student.GetPreferences().First();
        var lecturer = project.GetOwner()!;

        var teacherPreferences = lecturer.GetPreferences();
        if (!teacherPreferences.Contains(student))
        {
            student.GetPreferences().Remove(project);
            return; //cтудент не является примлемым для препода
        }

        AllocationUtils.createAssignment(student, project);

        var studentsByProject = project.GetAssigned();
        var studentsByLecturer = lecturer.GetAssigned();

        if (studentsByProject.Count > project.GetCapacity())
        {
            var worstStudent = findWorstStudentByProject(project);
            AllocationUtils.breakAssignment(project, worstStudent);
        }

        if (studentsByLecturer.Count > lecturer.GetCapacity())
        {
            var worstStudent = findWorstStudentByLecturer(lecturer);
            var projectWithWorstStudent = (IDependentAllocated)worstStudent.GetAssigned()!;
            AllocationUtils.breakAssignment(projectWithWorstStudent, worstStudent);
        }

        if (studentsByProject.Count == project.GetCapacity())
        {
            var worstStudent = findWorstStudentByProject(project);
            deleteSuccessors(worstStudent, lecturer, project);
        }


        if (studentsByLecturer.Count == lecturer.GetCapacity())
        {
            var worstStudent = findWorstStudentByLecturer(lecturer);
            deleteSuccessors(worstStudent, lecturer);
        }
    }

    private static void deleteSuccessors(IToOneAllocated student, ITwoStepAllocated lecturer)
    {
        var lecturerPreferences = new List<IToOneAllocated>(lecturer.GetPreferences().Cast<IToOneAllocated>());
        var lecturerProjects = lecturer.GetProjects();
        var studentIndex = lecturerPreferences.FindIndex(allocated => Equals(allocated, student));
        var successors = lecturerPreferences
            .GetRange(studentIndex + 1, lecturerPreferences.Count - studentIndex - 1);
        successors.ForEach(successor =>
            {
                var desiredProjectsOfThisLecturer = successor.GetPreferences()
                    .Where(project => lecturerProjects.Contains(project)).ToList();

                foreach (var project in desiredProjectsOfThisLecturer) successor.GetPreferences().Remove(project);
            }
        );
    }

    private static void deleteSuccessors(IToOneAllocated student, ITwoStepAllocated lecturer,
        IDependentAllocated project)
    {
        var lecturerPreferences = new List<IToOneAllocated>(lecturer.GetPreferences().Cast<IToOneAllocated>());

        var studentsReachableForThisProject = lecturerPreferences
            .Where(allocated => allocated.GetPreferences().Contains(project)).ToList();
        var studentIndex = studentsReachableForThisProject.FindIndex(allocated => Equals(allocated, student));
        if (studentIndex == -1)
            return;
        var successors = studentsReachableForThisProject.GetRange(studentIndex + 1,
            studentsReachableForThisProject.Count - studentIndex - 1
        );
        successors.ForEach(successor => { successor.GetPreferences().Remove(project); });
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