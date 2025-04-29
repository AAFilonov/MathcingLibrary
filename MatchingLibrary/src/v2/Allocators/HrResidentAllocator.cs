using MatchingLibrary.v2.Allocated;
using MatchingLibrary.v2.Allocation;
using MatchingLibrary.v2.Allocators.interfaces;

namespace MatchingLibrary.v2.Allocators;

public class HrResidentAllocator : IAllocator<IOneToManyAllocation>
{
    public void computeIteration(IOneToManyAllocation allocation)
    {
        foreach (var resident in allocation.GetResidents())
            if (haveReachablePair(resident))
                findPair(allocation, resident);
    }

    public bool isFinal(IOneToManyAllocation allocation)
    {
        var freeStudents = allocation.GetResidents().Where(s => haveReachablePair(s));
        return !freeStudents.Any();
    }

    private void findPair(IOneToManyAllocation allocation, IToOneAllocated resident)
    {
        var residentPreferences = resident.GetPreferences();

        if (!residentPreferences.Any())
            return; //список предпочтений пуст

        var hospital = (IToManyAllocated) residentPreferences.First();
        var hospitalPreferences = hospital.GetPreferences();
        if (!hospitalPreferences.Contains(resident))
        {
            resident.GetPreferences().Remove(hospital);
            return; //cтудент не является примлемым для препода   
        }

        AllocationUtils.createAssignment(resident, hospital);
        var assignedResidents = hospital.GetAssigned();
        if (assignedResidents.Count > hospital.GetCapacity())
        {
            var worstAssignedStudent = findWorstAssignedStudent(hospital, allocation);
            AllocationUtils.breakAssignment(hospital, worstAssignedStudent);
            worstAssignedStudent.GetPreferences().Remove(hospital);
        }
        else if (assignedResidents.Count == hospital.GetCapacity())
        {
            IAllocated worstAssignedStudent = findWorstAssignedStudent(hospital, allocation);
            deleteSuccessors(hospitalPreferences, worstAssignedStudent);
        }
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