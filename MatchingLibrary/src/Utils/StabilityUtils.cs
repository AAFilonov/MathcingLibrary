using MatchingLibrary.Allocated;
using MatchingLibrary.Allocation;

public static class StabilityUtils
{
    public static bool IsStable(IOneToManyAllocation allocation)
    {
        var residents = allocation.GetResidents();
        var hospitals = allocation.GetHospitals(); // или GetHospitals()

        foreach (var resident in residents)
        {
            var residentPreferences = resident.GetPreferences();

            foreach (var hospital in hospitals)
            {
                // проверяем только если hospital есть в предпочтениях
                if (!residentPreferences.Contains(hospital))
                    continue;

                var currentHospital = resident.GetAssigned() as IToManyAllocated;

                // 1️⃣ предпочитает ли resident этот hospital текущему?
                bool residentPrefers =
                    currentHospital == null ||
                    Prefers(residentPreferences, hospital, currentHospital);

                if (!residentPrefers)
                    continue;

                // 2️⃣ hospital готов ли принять?
                var assigned = hospital.GetAssigned();

                // свободная квота
                if (assigned.Count < hospital.GetCapacity())
                    return false;

                // либо hospital предпочитает resident худшему назначенному
                var worstAssigned = FindWorstAssigned(hospital);

                if (Prefers(hospital.GetPreferences(), resident, worstAssigned))
                    return false;
            }
        }

        return true;
    }

    private static bool Prefers(List<IAllocated> preferences,
                                IAllocated better,
                                IAllocated worse)
    {
        return preferences.IndexOf(better) <
               preferences.IndexOf(worse);
    }

    private static IToOneAllocated FindWorstAssigned(IToManyAllocated hospital)
    {
        var preferences = hospital.GetPreferences();
        var assigned = hospital.GetAssigned();

        IToOneAllocated worst = null;
        int worstIndex = -1;

        foreach (var resident in assigned)
        {
            var index = preferences.IndexOf(resident);
            if (index > worstIndex)
            {
                worstIndex = index;
                worst = resident as IToOneAllocated;
            }
        }

        return worst;
    }
}