using System.Linq.Expressions;
using MatchingLibrary.Algorithms.interfaces;
using MatchingLibrary.Allocation.interfaces;

namespace MatchingLibrary.Algorithms.impl;

public class HrAlgorithm<S, L>  :IOneToManyAllocationAlgorithm<S,L>
    where S : class
    where L : class

{
    public void computeIteration(IOneToManyAllocation<S, L> allocation)
    {
        foreach (var student in allocation.getListS())
        {
            if (haveReachablePair(student,allocation)) 
                findPair(allocation, student);
        }
    }

    private void findPair(IOneToManyAllocation<S, L> allocation, S student)
    {
        var studetnPreferences = allocation.getStudentPreferences(student);

        if (!studetnPreferences.Any())
            return; //список предпочтений пуст

        var lecturer = studetnPreferences.First();
        var teacherPreferences = allocation.getLecturersPreferences(lecturer);
        if (!teacherPreferences.Contains(student))
        {
            allocation.deleteFromStudentPreferences(student,lecturer);
            return; //cтудент не является примлемым для препода   
        }


        allocation.assign(lecturer,student);
        var assignedStudents = allocation.getAssignedByL(lecturer);
        if (assignedStudents.Count > allocation.getLecturerCapacity(lecturer))
        {
            S worstAssignedStudent = findWorstAssignedStudent(lecturer, allocation);
            allocation.breakAssigment(worstAssignedStudent,lecturer);
        }
        else if (assignedStudents.Count ==  allocation.getLecturerCapacity(lecturer))
        {
            S worstAssignedStudent = findWorstAssignedStudent(lecturer, allocation);
            deleteSuccessors(teacherPreferences, worstAssignedStudent);
        }
    }

    private static void deleteSuccessors(List<S> teacherPreferences, S worstAssignedStudent)
    {
        int worstAssignedStudentIndex =
            teacherPreferences.FindIndex(allocated => allocated == worstAssignedStudent);
        teacherPreferences.RemoveRange(worstAssignedStudentIndex + 1,
            teacherPreferences.Count - worstAssignedStudentIndex - 1);
    }
    

    private S findWorstAssignedStudent(L lecturer, IOneToManyAllocation<S, L> allocation)
    {
        List<S> preferences = new List<S>(allocation.getLecturersPreferences(lecturer));
        preferences.Reverse(); //список по возрастанию

        S worstStudent = null;
        int worstScore = preferences.Count; //меньше = хуже
        allocation.getAssignedByL(lecturer).ForEach(student =>
        {
            var score = preferences.FindIndex(allocated => allocated == student);
            if (score < worstScore)
            {
                worstStudent = student;
                worstScore = score;
            }
        });
        return worstStudent;
    }


    public bool isFinal(IOneToManyAllocation<S, L> allocation)
    {
        var freeStudents = allocation.getListS().Where(s => haveReachablePair(s, allocation));
        return !freeStudents.Any();
    }

    private bool haveReachablePair(S s, IOneToManyAllocation<S, L> allocation)
    {
        bool havePair = allocation.getAssignedByS(s)!=null;
        bool canFindPair = allocation.getStudentPreferences(s).Any();
        return !havePair && canFindPair;
    }
}