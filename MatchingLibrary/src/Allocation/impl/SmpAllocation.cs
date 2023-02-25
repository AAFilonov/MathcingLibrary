using MatchingLibrary.Allocation.interfaces;
using MatchingLibrary.Utils;

namespace MatchingLibrary.Allocation.impl;

public class SmpAllocation<M, W> :ISmpAllocation<M,W>
    where M : class
    where W : class
{
    public SmpAllocation(List<M> listT, List<W> listU)
    {
        this.listT = listT;
        this.listU = listU;
    }

    private List<M> listT { get; set; }
    private List<W> listU { get; set; }

    private SetOfPairs<W, M> uPreferencePairs { get; set; } = new();

    private SetOfPairs<M, W> tPreferencePairs { get; set; } = new();
    private SetOfPairs<M, W> assigmentPairs { get; set; } = new();


    public List<M> getMList()
    {
        return listT;
    }
    public List<W> getWList()
    {
        return listU;
    }
    public void assign(M m, W w)
    {
        assigmentPairs.Add(m, w);
    }

    public void breakAssigment(M m, W w)
    {
        assigmentPairs.Remove(m, w);
    }

    public W? getAssignedByM(M m)
    {
        return assigmentPairs.getByKey(m);
    }
    public M? getAssignedByW(W w)
    {
        return assigmentPairs.getByValue(w);
    }


    public List<(M, W)> calcFinalAllocation()
    {
        var result = new List<(M, W)>();
        foreach (var pair in this.assigmentPairs.pairs)
        {
            result.Add(new(pair.Item1, pair.Item2));
        }

        return result;
    }

    public List<W> getTPreferences(M t)
    {
        return tPreferencePairs.getManyByKey(t);
    }

    public List<M> getUPreferences(W u)
    {
        return uPreferencePairs.getManyByKey(u);
    }

    public void setTPreferences(M t, List<W> pref)
    {
        foreach (var u in pref)
        {
            setTPreferencePair(t, u);
        }
    }

    public void setTPreferencePair(M t, W u)
    {
        tPreferencePairs.Add(t, u);
    }

    public void setUPreferences(W u, List<M> pref)
    {
        foreach (var t in pref)
        {
            setUPreferencePair(u, t);
        }
    }

    public void setUPreferencePair(W u, M t)
    {
        uPreferencePairs.Add(u, t);
    }

    public void deleteFromTPref(M t, W u)
    {
        tPreferencePairs.Remove(t, u);
    }
}