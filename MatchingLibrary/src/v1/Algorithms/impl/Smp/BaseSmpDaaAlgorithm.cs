using System.Linq;
using MatchingLibrary.v1.Algorithms.interfaces;
using MatchingLibrary.v1.Allocation.interfaces.Smp;

namespace MatchingLibrary.v1.Algorithms.impl.Smp;

public class DAAAlgorithm<M, W> : IOneToOneGenericAllocationAlgorithm<M, W>
    where M : class
    where W : class
{
    public void computeIteration(ISmpAllocationGeneric<M, W?> allocationGeneric)
    {
        foreach (var freeMan in allocationGeneric.getMList())
        {
            if (HaveReacheblePair(freeMan, allocationGeneric))
                findPair(allocationGeneric, freeMan);
        }
    }

    private void findPair(ISmpAllocationGeneric<M, W?> allocationGeneric, M freeMan)
    {
        var DesiredWomen = allocationGeneric.getTPreferences(freeMan);
        if (!DesiredWomen.Any())
            return; //список предпочтений пуст
        var DesiredWoman = DesiredWomen.First();

        var womanPref = allocationGeneric.getUPreferences(DesiredWoman);
        var womanPair = allocationGeneric.getAssignedByW(DesiredWoman);
        if (womanPair == null)
        {
            allocationGeneric.assign(freeMan, DesiredWoman);
        }
        else if (womanPref.IndexOf(freeMan) > -1 &&
                 womanPref.IndexOf(freeMan) < womanPref.IndexOf(womanPair)) //новый мужчина лучше предыдущего
        {
            allocationGeneric.assign(freeMan, DesiredWoman);
            allocationGeneric.deleteFromMPref(womanPair, DesiredWoman);
        }
        else
        {
            allocationGeneric.deleteFromMPref(freeMan, DesiredWoman);
        }
    }
    
    public bool isFinal(ISmpAllocationGeneric<M, W?> allocationGeneric)
    {
        var freeMen = allocationGeneric.getMList().Where(allocated => HaveReacheblePair(allocated, allocationGeneric));
        return !freeMen.Any();
    }

    private bool HaveReacheblePair(M allocated, ISmpAllocationGeneric<M, W?> allocationGeneric)
    {
        bool havePair = allocationGeneric.getAssignedByM(allocated) != null;
        bool canFindPair = allocationGeneric.getTPreferences(allocated).Any();
        return !havePair && canFindPair;
    }
}