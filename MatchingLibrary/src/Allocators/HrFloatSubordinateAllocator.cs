using MatchingLibrary.Allocated;
using MatchingLibrary.Allocation;
using MatchingLibrary.Allocators.interfaces;
using MatchingLibrary.Utils;

namespace MatchingLibrary.Allocators;

/*
 *Алогритм предложенный в раболте
 * 1. Liu D. [и др.]. Task-Based Network Reconfiguration in Distributed UAV Swarms: A Bilateral Matching Approach
 * // IEEE/ACM Transactions on Networking. 2022. № 6 (30). C. 2688–2700.
 * Отличается от классического алгоритма Гейла-Шепли динамической кратностью связи - квоты ведущих
 * являются вещественной величиной.
 * Данная реализация порождает распределение, оптимальное для подчиненного актора.
 *  */
public class HrFloatSubordinateAllocator : IAllocator<IOneToManyFloatAllocation>
{
    public void computeIteration(IOneToManyFloatAllocation allocation)
    {
        foreach (var subordinate in allocation.GetSubordinates())
            if (haveReachablePair(subordinate))
                findPair(allocation, subordinate);
    }

    private void findPair(IOneToManyFloatAllocation allocation, IToOneFloatAllocated m)
    {
        var subordinatePreferences = m.GetPreferences();

        if (!subordinatePreferences.Any())
            return; //список предпочтений пуст

        var n = (IToManyFloatAllocated)subordinatePreferences.First();
        var nPreferences = n.GetPreferences();
        if (!nPreferences.Contains(m))
        {
            m.GetPreferences().Remove(n);
            return; //подчиненный не является приемлемым для ведущего  
        }

        double free_quota_q = n.GetFreeCapacity();

        if (free_quota_q < m.GetRequestedQuota())
        {
            allocation.refreshPreferences(m);
           // m.GetPreferences().Remove(n);
           //подчиненный обновляет предпочтения - возможно данный ведущий все еще лучший 
        }
        /*
        AllocationUtils.createAssignment(m, n);
        var assignedResidents = n.GetAssigned();
        if (assignedResidents.Count > n.GetCapacity())
        {
            var worstAssignedStudent = findWorstAssignedStudent(n, allocation);
            AllocationUtils.breakAssignment(n, worstAssignedStudent);
            worstAssignedStudent.GetPreferences().Remove(n);
        }
        else if (assignedResidents.Count == n.GetCapacity())
        {
            IAllocated worstAssignedStudent = findWorstAssignedStudent(n, allocation);
            deleteSuccessors(nPreferences, worstAssignedStudent);
        }
        */
        //TODO реализовать алгоритм
        //TODO реализовать обратный алгоритм
    }

    public bool isFinal(IOneToManyFloatAllocation allocation)
    {
        var freeStudents = allocation.GetSubordinates().Where(s => haveReachablePair(s));
        return !freeStudents.Any();
    }


    private static void deleteSuccessors(List<IAllocated> teacherPreferences, IAllocated worstAssignedStudent)
    {
        var worstAssignedStudentIndex =
            teacherPreferences.FindIndex(allocated => allocated == worstAssignedStudent);
        teacherPreferences.RemoveRange(worstAssignedStudentIndex + 1,
            teacherPreferences.Count - worstAssignedStudentIndex - 1);
    }

    private IToOneAllocated findWorstAssignedStudent(IToManyAllocated hospital, IOneToManyFloatAllocation allocation)
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

    private bool haveReachablePair(IToOneFloatAllocated resident)
    {
        var havePair = resident.GetAssigned() != null;
        var canFindPair = resident.GetPreferences().Any();
        return !havePair && canFindPair;
    }
}