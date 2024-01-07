using MatchingLibrary.v2.Allocated;
using MatchingLibrary.v2.Allocation;
using MatchingLibrary.v2.Allocators.interfaces;

namespace MatchingLibrary.v2.Allocators;

public class TwoStepAllocator: IAllocator<ITwoStepAllocation>
{
    public void  computeIteration(ITwoStepAllocation allocation)
    {
        foreach (var student in allocation.GetStudents())
        {
            if (haveReachablePair(student)) 
                findPair(student);
        }
    }

    private void findPair(IToOneAllocated student)
    {
        /*
        IToManyAllocated project =  (IToManyAllocated) student.GetPreferences().First();
        IToManyAllocated lecturer = allocation.getLecturerByProject(project);
        
        var teacherPreferences = lecturer.GetPreferences();
        if (!teacherPreferences.Contains(student))      
        {
            student.GetPreferences().Remove(project);
            return; //cтудент не является примлемым для препода   
        }

        
        allocation.assign(student, project);
        var studentsByProject = allocation.getAssignedByP(project);
        var studentsByLecturer = allocation.getAssignedByL(lecturer);

        if (studentsByProject.Count >allocation.getProjectCapacity(project))
        {
            S worstStudent = findWorstStudentByProject(allocation, project);
            allocation.breakAssigment(worstStudent, project);
        }

        if (studentsByLecturer.Count > allocation.getLecturerCapacity(lecturer))
        {
            S worstStudent = findWorstStudentByLecturer(allocation, lecturer);
            P projectWithWorstStudent = allocation.getProjectByAssigned(worstStudent) ?? throw new InvalidOperationException();
            allocation.breakAssigment(worstStudent, projectWithWorstStudent);
        }

        if (studentsByProject.Count == allocation.getProjectCapacity(project))
        {
            S worstStudent = findWorstStudentByProject(allocation, project);
            deleteSuccessors(allocation, worstStudent, lecturer ,project);
        }

        if (studentsByLecturer.Count == allocation.getLecturerCapacity(lecturer))
        {
            S worstStudent = findWorstStudentByLecturer(allocation, lecturer);
            deleteSuccessors(allocation, worstStudent, lecturer);
        }*/
    }
/*
    private void deleteSuccessors(ITwoStepAllocation<S, L, P> allocation, S student, L lecturer)
    {
        var preferences = allocation.getLecturerPreferences(lecturer);
        var lecturerProjects = allocation.getProjects(lecturer);
        int studentIndex = preferences.FindIndex(allocated => allocated == student);
        List<S> successors = preferences
            .GetRange(studentIndex + 1, preferences.Count - studentIndex - 1);
        successors.ForEach(successor =>
        {
          var desiredProjectsOfThisLecturer= allocation.getStudentPreferences(successor)
              .Where(project => lecturerProjects.Contains(project));

          foreach (var project in desiredProjectsOfThisLecturer)
          {
              allocation.deleteStudentPreferencePair(successor, project);  
          }
        });
    }

    private void deleteSuccessors(ITwoStepAllocation<S, L, P> allocation, S student, L lecturer, P project)
    {
        List<S> preferences = allocation.getLecturerPreferences(lecturer);
        
        var studentsReachableForThisProject = preferences
            .Where(allocated => allocation.getStudentPreferences(allocated).Contains(project)).ToList();
        int studentIndex = studentsReachableForThisProject.FindIndex(allocated => allocated == student);
        if (studentIndex == -1)
            return;
        var successors = studentsReachableForThisProject.GetRange(studentIndex + 1, studentsReachableForThisProject.Count - studentIndex - 1);
        successors.ForEach(successor => { allocation.deleteStudentPreferencePair(successor, project); });
    }

    private S findWorstStudentByLecturer(ITwoStepAllocation<S, L, P> allocation, L lecturer)
    {
        List<S> preferences = new List<S>(allocation.getLecturerPreferences(lecturer));
        var assignedStudents = allocation.getAssignedByL(lecturer);

        bool AllNotAssigned(S allocated) => !assignedStudents.Contains(allocated);
        
        preferences.RemoveAll(AllNotAssigned);
        return preferences.Last();
    }

    private S findWorstStudentByProject(ITwoStepAllocation<S, L, P> allocation, P project)
    {
        L lecturer = allocation.getLecturerByProject(project);
        List<S> preferences = new List<S>(allocation.getLecturerPreferences(lecturer));
        preferences.Reverse(); //sort in ascending order.


        var assignedStudents = allocation.getAssignedByP(project);
        S worstStudent = assignedStudents.First();
        int worstScore = preferences.Count; //less is worse = хуже
        foreach (var student in assignedStudents)
        {
            var score = preferences.FindIndex(allocated => allocated == student);
            if (score < worstScore)
            {
                worstStudent = student;
                worstScore = score;
            }
        };
        return worstStudent;
    }
    */
    public bool isFinal(ITwoStepAllocation allocation)
    {
        var freeStudents = allocation.GetStudents().Where(s => haveReachablePair(s));
        return !freeStudents.Any();
    }

    private bool haveReachablePair(IToOneAllocated student)
    {
        bool havePair = student.GetAssigned() != null;
        bool canFindPair = student.GetPreferences().Any();
        return !havePair && canFindPair;
    }
}