using MatchingLibrary.Algorithms.interfaces;
using MatchingLibrary.Allocation.impl;
using MatchingLibrary.Allocation.interfaces;

namespace MatchingLibrary.Algorithms.impl;

public class SpAlgorithm<S, L, P> :ITwoStepAllocationAlgorithm  <S,L,P>
    where S : class
    where L : class
    where P : class
{
    public void  computeIteration(ITwoStepAllocation<S, L, P> allocation)
    {
        foreach (var student in allocation.getListS())
        {
            if (haveReachablePair(student,allocation)) 
                findPair(allocation, student);
        }
    }

    private void findPair(ITwoStepAllocation<S, L, P> allocation, S student)
    {
        P project =  allocation.getStudentPreferences(student).First();
        L lecturer = allocation.getLecturerByProject(project);
        
        var teacherPreferences = allocation.getLecturerPreferences(lecturer);
        //КОСТЫЛЬ - из за того что мы допускаем что предпочтения препода не являются полными
        //на множестве достижимых студентов,возможна ситуация когда препод (проект)
        //будет в предпочтениях студента, а студент в предпочтениях препода не будет 
        //в фактическом распределении студент назначается преподователю в список ожидания а после преподователь его сортирует
        //условие никогда не выполнится если бы предпочтения преподоавтеля были бы полными
        if (!teacherPreferences.Contains(student))      
        {
            allocation.deleteFromStudentPref(student,project);
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
        }
    }

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


    public bool isFinal(ITwoStepAllocation<S, L, P> allocation)
    {
        var freeStudents = allocation.getListS().Where(s => haveReachablePair(s, allocation));
        return !freeStudents.Any();
    }

    private bool haveReachablePair(S student, ITwoStepAllocation<S, L, P> allocation)
    {
        bool havePair = allocation.getProjectByAssigned(student) != null;
        bool canFindPair = allocation.getStudentPreferences(student).Any();
        return !havePair && canFindPair;
    }
}