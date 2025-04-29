using System.Collections.Generic;
using System.Linq;
using MatchingLibrary.v1.Allocation.interfaces;

namespace MatchingLibrary.v3;

public class HrAlgorithm<H,R>
{
    public void computeFinalAllocation(
        List<Allocated<H>> allocatedTutors,
        List<Allocated<R>> allocatedStudents
    )
    {
        while (!isFinal(allocatedStudents))
        {
            computeIteration(allocatedTutors, allocatedStudents);
        }
    }
    public void computeIteration(
        List<Allocated<H>> allocatedTutors,
        List<Allocated<R>> allocatedStudents
        )
    {
        foreach (var student in allocatedStudents){
            if (haveReachablePair(student)) 
                findPair(student);
        }
    }

    private void findPair(Allocated<R> student)
    {
        var studetnPreferences = student.preferences;

        if (!studetnPreferences.Any())
            return; //список предпочтений пуст

        Allocated<H> lecturer = (Allocated<H>) studetnPreferences.First();
        List<object> teacherPreferences =lecturer.preferences;
        if (!teacherPreferences.Contains(student))
        {
            student.preferences.Remove(lecturer);
            return; //cтудент не является примлемым для препода   
        }
        
        lecturer.assigned.Add(student);
        student.assigned.Add(lecturer);

        if (lecturer.assigned.Count >lecturer.capacity)
        {
            //break assigment
            Allocated<R> worstAssignedStudent = findWorstAssignedStudent(teacherPreferences,lecturer);
            worstAssignedStudent.assigned.Remove(lecturer);
            lecturer.assigned.Remove(worstAssignedStudent);
            worstAssignedStudent.preferences.Remove(lecturer);
            lecturer.preferences.Remove(worstAssignedStudent);
            
        }
        else if (lecturer.assigned.Count == lecturer.assigned.Capacity)
        {
            Allocated<R> worstAssignedStudent = findWorstAssignedStudent(teacherPreferences,lecturer);
            deleteSuccessors(teacherPreferences, worstAssignedStudent);
        }
    }

    private static void deleteSuccessors(List<object> teacherPreferences, Allocated<R> worstAssignedStudent)
    {
        int worstAssignedStudentIndex =
            teacherPreferences.FindIndex(allocated => allocated == worstAssignedStudent);
        teacherPreferences.RemoveRange(worstAssignedStudentIndex + 1,
            teacherPreferences.Count - worstAssignedStudentIndex - 1);
    }
    

    private  Allocated<R> findWorstAssignedStudent(List<object> teacherPreferences,Allocated<H> lecturer)
    {
        List<Allocated<R>> preferences = lecturer.GetAssigned<R>();
        //preferences.Reverse(); //список уже по возрастанию

        Allocated<R> worstStudent = null;
        int worstScore = preferences.Count; //меньше = хуже
        lecturer.GetAssigned<R>().ForEach(student =>
        {
            var score = lecturer.GetAssigned<R>().FindIndex(allocated => allocated == student);
            if (score < worstScore)
            {
                worstStudent = student;
                worstScore = score;
            }
        });
        return worstStudent;
    }


    public bool isFinal(List<Allocated<R>> allocatedStudents)
    {
        var freeStudents = allocatedStudents.Where(s => haveReachablePair(s));
        return !freeStudents.Any();
    }

    private bool haveReachablePair(Allocated<R> r)
    {
        bool havePair = r.assigned.Any();
        bool canFindPair = r.preferences.Any();
        return !havePair && canFindPair;
    }
}