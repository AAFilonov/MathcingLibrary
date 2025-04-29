using System.Linq;
using MatchingLibrary.v2.Allocated;
using MatchingLibrary.v2.Allocation;
using MatchingLibrary.v2.Allocators.interfaces;

namespace MatchingLibrary.v2.Allocators;

//Алгоритм отложенного принятия (DAA) ориентированный на мужчин
public class SmpDaaMaleAllocator : IOneToOneAllocator
{
    public void computeIteration(IOneToOneAllocation allocation)
    {
        foreach (var freeMan in allocation.GetMen())
            if (HaveReacheblePair(freeMan))
                findPair(allocation, freeMan);
    }

    public bool isFinal(IOneToOneAllocation allocation)
    {
        return !allocation
            .GetMen()
            .Any(HaveReacheblePair);
    }

    private void findPair(IOneToOneAllocation allocation, IToOneAllocated freeMan)
    {
        var manPreferences = freeMan.GetPreferences();
        if (!manPreferences.Any())
            return; //список предпочтений пуст
        var desiredWoman = manPreferences.First();

        var womanPref = desiredWoman.GetPreferences();
        var womanPair = (IAllocated?) desiredWoman.GetAssigned();

        if (!womanPref.Contains(freeMan))
        {
            freeMan.GetPreferences().Remove(desiredWoman);
            return; //Мужчина не является допустимой парой
        }

        if (womanPair == null)
        {
            freeMan.Assign(desiredWoman);
            desiredWoman.Assign(freeMan);
        }
        else if (womanPref.IndexOf(freeMan) > -1 &&
                 womanPref.IndexOf(freeMan) < womanPref.IndexOf(womanPair)) //новый мужчина лучше предыдущего
        {
            desiredWoman.Assign(freeMan);
            desiredWoman.GetPreferences().Remove(womanPair);
            womanPair.Assign(null);
            womanPair.GetPreferences().Remove(desiredWoman);
        }
        else
        {
            freeMan.GetPreferences().Remove(desiredWoman);
        }
    }

    private bool HaveReacheblePair(IToOneAllocated toOneAllocated)
    {
        var havePair = toOneAllocated.GetAssigned() != null;
        var canFindPair = toOneAllocated.GetPreferences().Any();
        return !havePair && canFindPair;
    }
}