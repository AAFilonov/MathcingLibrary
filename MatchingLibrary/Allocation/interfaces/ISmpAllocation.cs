

namespace MatchingLibrary.Allocation.interfaces;

//allocation for classic Stable Marriage Problem
public interface ISmpAllocation<M, W> 
    where M : class
    where W : class
{
    
    void assign(M m, W w);
    void breakAssigment(M m, W w);
    W? getAssignedByM(M m);
    M? getAssignedByW(W w);
    
    List<M> getMList();
    List<W> getWList();
    List<W> getTPreferences(M t);
    List<M> getUPreferences(W u);

    void setTPreferences(M t, List<W> pref);
    void setTPreferencePair(M t, W u);
    void setUPreferences(W u, List<M> pref);
    void setUPreferencePair(W u, M t);
    void deleteFromTPref(M t, W u);
}