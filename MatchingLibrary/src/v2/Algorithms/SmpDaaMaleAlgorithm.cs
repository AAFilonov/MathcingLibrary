using MatchingLibrary.v2.Allocated;
using MatchingLibrary.v2.Allocation;

namespace MatchingLibrary.v2.Algorithms;

//Алгоритм отложенного принятия (DAA) ориентированный на мужчин
public class SmpDaaMaleAlgorithm : IOneToOneAllocationAlgorithm
{
    public void computeIteration(ISmpAllocation allocation)
    {
        foreach (var freeMan in allocation.getMen())
        {
            if (HaveReacheblePair(freeMan))
                findPair(allocation, freeMan);
        }
    }

    private void findPair(ISmpAllocation allocation, IOneToOneAllocated freeMan)
    {
        List<IOneToOneAllocated> manPreferences = freeMan.GetPreferences();
        if (!manPreferences.Any())
            return; //список предпочтений пуст
        IOneToOneAllocated desiredWoman = manPreferences.First();

        var womanPref = desiredWoman.GetPreferences();
        var womanPair = desiredWoman.GetAssigned();
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
    
    public bool isFinal(ISmpAllocation allocation)
    {
        return !allocation
            .getMen()
            .Any(HaveReacheblePair);
    }
    private bool HaveReacheblePair(IOneToOneAllocated oneToOneAllocated)
    {
        bool havePair = oneToOneAllocated.GetAssigned() != null;
        bool canFindPair = oneToOneAllocated.GetPreferences().Any();
        return !havePair && canFindPair;
    }
}