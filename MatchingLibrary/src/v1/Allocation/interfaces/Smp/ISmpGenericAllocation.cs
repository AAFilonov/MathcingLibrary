

namespace MatchingLibrary.v1.Allocation.interfaces.Smp;

//allocation for classic Stable Marriage Problem
public interface ISmpAllocationGeneric<M, W> 
    where M : class
    where W : class
{
    
    void assign(M m, W? w);
    void breakAssigment(M m, W? w);
    W? getAssignedByM(M m);
    M? getAssignedByW(W? w);
    
    List<M> getMList();
    List<W> getWList();
    List<W> getTPreferences(M t);
    List<M> getUPreferences(W u);

    void setTPreferences(M t, List<W> pref);
    void setTPreferencePair(M m, W u);
    void setUPreferences(W u, List<M> pref);
    void setUPreferencePair(W u, M t);
    void deleteFromMPref(M t, W u);
}