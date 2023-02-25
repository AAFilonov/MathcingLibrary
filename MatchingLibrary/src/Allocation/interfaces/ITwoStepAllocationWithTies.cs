namespace MatchingLibrary.Allocation.interfaces;

public interface ITwoStepAllocationWithTies<S,L,P>
    where S : class
    where L : class
    where P : class
{
    
    void assign(S s, P p);
    void breakAssigment(S s, P p);
    List<S> getAssigned(P p);
    List<S> getAssigned(L l);
    
    List<S> getStudents();
    void setLecturerCapacity(L l,int c);
    void seProjectCapacity(P p,int c);
    int getLecturerCapacity(L l);
    int getProjectCapacity(P p);
    void setProjects(L lecturer, List<P> projects);
    List<P> getProjects(L lecturer);
    void setProject(L lecturer, P project);
    L getLecturerByProject(P project);

    List<List<P>> getStudentPreferences(S student);
    List<List<S>> getLecturerPreferences(L lecturer);
    void setStudentPreferences(S student, List< List<P>> pref);
    void setStudentPreferencePair(S student,  List<P> projectTie);
    void deleteStudentPreferencePair(S student,  List<P> projectTie);
    void setLecturerPreferences(L lecturer, List<List<S>> studnetTies);

    P? getProjectByAssigned(S s);
    void setLecturerPreferencePair(L lecturer, List<S> studentTie);
    void deleteFromStudentPref(S student, List<P> projectsTie);
    List<(L, List<(P, List<S>)>)> calcFinalAllocation();
}