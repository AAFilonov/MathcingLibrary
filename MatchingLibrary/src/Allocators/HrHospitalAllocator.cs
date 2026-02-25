using MatchingLibrary.Allocated;
using MatchingLibrary.Allocation;
using MatchingLibrary.Allocators.interfaces;
using MatchingLibrary.Utils;

namespace MatchingLibrary.Allocators;

public class HrHospitalAllocator : IAllocator<IOneToManyAllocation>
{
    public void computeIteration(IOneToManyAllocation allocation)
    {
        foreach (var hospital in allocation.GetHospitals())
            while (haveReachablePair(hospital))
            {
                findPair(allocation, hospital);
            }
    }

    private bool haveReachablePair(IToManyAllocated hospital)
    {
        var quotaIsNotFilled = hospital.GetAssigned().Count < hospital.GetCapacity();
        var canFindPair = hospital.GetPreferences().Except(hospital.GetAssigned()).Any();
        return quotaIsNotFilled && canFindPair;
    }

    private void findPair(IOneToManyAllocation allocation, IToManyAllocated hospital)
    {
        var hospitalAssigned = hospital.GetAssigned();
        var hospitalPreferences = hospital.GetPreferences().Except(hospitalAssigned);
        if (!hospitalPreferences.Any())
            return; //список предпочтений пуст

        var resident = (IToOneAllocated)hospitalPreferences.First();
        var residentPreferences = resident.GetPreferences();
        if (!residentPreferences.Contains(hospital))
        {
            hospital.GetPreferences().Remove(resident);
            return; //госпиталь не является приемлемым для резидента   
        }

        if (resident.GetAssigned() == null)
        {
            AllocationUtils.createAssignment(resident, hospital);
            //у резидента все равно нет других назначений, ему назначается данный госпиталь
        }
        else
        {
            IToManyAllocated assignedHospital = (IToManyAllocated)resident.GetAssigned();
            int hospitalPriorityForResident = resident.GetPreferences().IndexOf(hospital);
            int assignedHospitalPriorityForResident = resident.GetPreferences().IndexOf(assignedHospital);
            if (hospitalPriorityForResident <
                assignedHospitalPriorityForResident) // порядок предпочтений - в начале более предпочтительные
            {
                AllocationUtils.breakAssignment(resident, assignedHospital);
                AllocationUtils.createAssignment(resident, hospital);
            }
            else
            {
                resident.GetPreferences().Remove(hospital);
                //госпиталь менее предпочтителен для резидента чем уже назначенный ему   
            }
        }
    }

    public bool isFinal(IOneToManyAllocation allocation)
    {
        var unmatchedHospitals = allocation.GetHospitals().Where(h => haveReachablePair(h));
        return !unmatchedHospitals.Any();
    }
}