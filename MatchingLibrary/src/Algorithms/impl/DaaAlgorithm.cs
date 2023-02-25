using System.Linq.Expressions;
using MatchingLibrary.Algorithms.interfaces;
using MatchingLibrary.Allocation.impl;
using MatchingLibrary.Allocation.interfaces;

namespace MatchingLibrary.Algorithms.impl;

public class DAAAlgorithm<M, W> : IOneToOneAllocationAlgorithm<M, W>
    where M : class
    where W : class
{
    public void computeIteration(ISmpAllocation<M, W> allocation)
    {
        foreach (var freeMan in allocation.getMList())
        {
            if (HaveReacheblePair(freeMan, allocation))
                findPair(allocation, freeMan);
        }
    }

    private void findPair(ISmpAllocation<M, W> allocation, M freeMan)
    {
        var DesiredWomen = allocation.getTPreferences(freeMan);
        if (!DesiredWomen.Any())
            return; //список предпочтений пуст
        var DesiredWoman = DesiredWomen.First();

        var womanPref = allocation.getUPreferences(DesiredWoman);
        var womanPair = allocation.getAssignedByW(DesiredWoman);
        if (womanPair == null)
        {
            allocation.assign(freeMan, DesiredWoman);
        }
        else if (womanPref.IndexOf(freeMan) > -1 &&
                 womanPref.IndexOf(freeMan) < womanPref.IndexOf(womanPair)) //новый мужчина лучше предыдущего
        {
            allocation.assign(freeMan, DesiredWoman);
            allocation.deleteFromTPref(womanPair, DesiredWoman);
        }
        else
        {
            allocation.deleteFromTPref(freeMan, DesiredWoman);
        }
    }
    
    public bool isFinal(ISmpAllocation<M, W> allocation)
    {
        var freeMen = allocation.getMList().Where(allocated => HaveReacheblePair(allocated, allocation));
        return !freeMen.Any();
    }

    private bool HaveReacheblePair(M allocated, ISmpAllocation<M, W> allocation)
    {
        bool havePair = allocation.getAssignedByM(allocated) != null;
        bool canFindPair = allocation.getTPreferences(allocated).Any();
        return !havePair && canFindPair;
    }
}