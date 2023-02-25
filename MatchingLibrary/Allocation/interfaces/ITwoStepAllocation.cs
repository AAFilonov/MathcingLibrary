
namespace MatchingLibrary.Allocation.interfaces;

public interface ITwoStepAllocation<S, L, P> 
    where S : class
    where L : class
    where P : class
{
    
    List<S> getAssignedByL(L l);

    List<S> getListS();
    void setLecturerCapacity(L l, int c);
    int getLecturerCapacity(L l);
    void setProjectCapacity(P p, int c);
    int getProjectCapacity(P p);
    void setProjects(L l, List<P> listP);
    void setProject(L l, P p);
    List<P> getProjects(L l);
    L getLecturerByProject(P p);
    List<P> getStudentPreferences(S s);
    List<S> getLecturerPreferences(L l);
    void setStudentPreferences(S s, List<P> pref);
    void setStudentPreferencePair(S s, P p);
    void deleteStudentPreferencePair(S s, P p);
    void setLecturerPreferences(L s, List<S> pref);

    P? getProjectByAssigned(S s);
    void setLecturerPreferencePair(L l, S s);
    void deleteFromStudentPref(S s, P p);
    void assign(S s, P p);
    void breakAssigment(S s, P p);
    List<S> getAssignedByP(P project);
}