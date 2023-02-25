using MatchingLibrary.Allocation.interfaces;
using MatchingLibrary.Utils;

namespace MatchingLibrary.Allocation.impl;

public class OneToManyAllocation<S, L> :IOneToManyAllocation<S,L>
    where S : class
    where L : class
{
    public OneToManyAllocation(List<S> students, List<L> lecturers)
    {
        this.students = students;
        this.lecturers = lecturers;
    }

    private List<S> students { get; set; }
    private List<L> lecturers { get; set; }


    private SetOfPairs<S, L> studentsPreference { get; set; } = new();

    private SetOfPairs<L, S> lecturersPreference { get; set; } = new();
    
    private SetOfPairs<L, S> assigmentPairs { get; set; } = new();

    private Dictionary<L, int> lecturersCapacity { get; set; } = new();


    public List<S> getListS()
    {
        return students;
    }
    public List<L> getListL()
    {
        return lecturers;
    }
    public void assign(L l,S s)
    {
        assigmentPairs.Add(l, s);
    }
    public void breakAssigment(S s, L l)
    {
        assigmentPairs.Remove(l, s);
    }
    
    public List<S> getAssignedByL(L l)
    {
        return assigmentPairs.getManyByKey(l);
    }
    public L? getAssignedByS(S s)
    {
        return assigmentPairs.getByValue(s);
    }
    
    public List<L> getStudentPreferences(S t)
    {
        return studentsPreference.getManyByKey(t);
    }

    public List<S> getLecturersPreferences(L u)
    {
        return lecturersPreference.getManyByKey(u);
    }

    public void setStudentPreferences(S s, List<L> pref)
    {
        foreach (var p in pref)
            setStudentPreferencePair(s, p);
    }

    public void deleteFromStudentPreferences(S s, L l)
    {
        studentsPreference.Remove(s,l);
    }

    public void setStudentPreferencePair(S s, L l)
    {
        studentsPreference.Add(s, l);
    }

    public void setLecturerPreferences(L l, List<S> pref)
    {
        foreach (var student in pref)
            setLecturerPreferencePair(l, student);
    }
    public void setLecturerPreferencePair(L lecturer, S student)
    {
        lecturersPreference.Add(lecturer, student);
    }

    public void setLecturerCapacity(L l, int c)
    {
        ExceptionUtils.checkPresense(l, lecturers);
        lecturersCapacity.Add(l, c);
    }
    public int getLecturerCapacity(L l)
    {
        ExceptionUtils.checkPresense(l, lecturers);
        return lecturersCapacity[l];
    }

    public List<(L, List<S>)> calcFinalAllocation()
    {
        var result = new List<(L, List<S>)>();
        
        foreach (var lecturer in lecturers)
        {
            var assigned = getAssignedByL(lecturer);
            result.Add(new(lecturer, assigned));
        }
        return result;
    }
}