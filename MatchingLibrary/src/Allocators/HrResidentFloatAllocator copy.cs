using MatchingLibrary.Allocated;
using MatchingLibrary.Allocation;
using MatchingLibrary.Allocators.interfaces;

namespace MatchingLibrary.Allocators;

public class HrResidentFloatAllocator : IAllocator<IOneToManyFloatAllocation>
{
    public void computeIteration(IOneToManyFloatAllocation allocation)
    {
        foreach (var resident in allocation.GetResidents())
            computeStep(resident, allocation);
    }

    public bool isFinal(IOneToManyFloatAllocation allocation)
    {
        var freeStudents = allocation.GetResidents().Where(s => haveReachablePair(s));
        return !freeStudents.Any();
    }

    public void computeStep(IToOneAllocated resident, IOneToManyFloatAllocation allocation)
    {
        if (haveReachablePair(resident))
            findPair(allocation, resident);
    }

    private void findPair(IOneToManyFloatAllocation allocation, IToOneAllocated resident)
    {
        var residentPreferences = resident.GetPreferences();
        // тут  будет алгоритм китайцев
    }

    private static void deleteSuccessors(List<IAllocated> teacherPreferences, IAllocated worstAssignedStudent)
    {
        var worstAssignedStudentIndex =
            teacherPreferences.FindIndex(allocated => allocated == worstAssignedStudent);
        teacherPreferences.RemoveRange(worstAssignedStudentIndex + 1,
            teacherPreferences.Count - worstAssignedStudentIndex - 1);
    }

    private IToOneAllocated findWorstAssignedStudent(IToManyAllocated hospital, IOneToManyAllocation allocation)
    {
        var preferences = new List<IAllocated>(hospital.GetPreferences());
        preferences.Reverse(); //список по возрастанию

        IToOneAllocated worstStudent = null;
        var worstScore = preferences.Count; //меньше = хуже
        hospital.GetAssigned().ForEach(student =>
        {
            var score = preferences.FindIndex(allocated => allocated == student);
            if (score < worstScore)
            {
                worstStudent = student as IToOneAllocated;
                worstScore = score;
            }
        });
        return worstStudent;
    }

    private bool haveReachablePair(IToOneAllocated resident)
    {
        var havePair = resident.GetAssigned() != null;
        var canFindPair = resident.GetPreferences().Any();
        return !havePair && canFindPair;
    }
}