

namespace MatchingLibrary.Allocation.interfaces;

//allocation for classic Hospitals Residents Problem
public interface IOneToManyAllocation<S,L>
    where S : class
    where L : class
{
    
    public List<S> getListS();
    void assign(L l,S s);
    void breakAssigment(S s, L l);
    List<S> getAssignedByL(L l);
    L? getAssignedByS(S s);

    List<L> getStudentPreferences(S t);
    List<S> getLecturersPreferences(L u);
    void setStudentPreferences(S s, List<L> pref);
    void deleteFromStudentPreferences(S s, L l);
    void setStudentPreferencePair(S s, L l);
    void setLecturerPreferences(L l, List<S> pref);
    void setLecturerPreferencePair(L lecturer, S student);
    void setLecturerCapacity(L l, int c);
    int getLecturerCapacity(L l);
}