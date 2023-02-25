using MatchingLibrary.Allocation.interfaces;
using MatchingLibrary.Utils;

namespace MatchingLibrary.Allocation.impl.TwoStep;

public class TwoStepAllocation<S, L, P> :ITwoStepAllocation<S,L,P>
    where S : class
    where L : class
    where P : class
{
    public TwoStepAllocation(List<S> students, List<L> lecturers, List<P> projects)
    {
        this.students = students;
        this.lecturers = lecturers;
        this.projects = projects;
    }

    //наблюдение - преференсы преподователе это упорядочивание множества всех студентов, считающих их /их проект примелемым        
    private List<S> students { get; }
    private List<L> lecturers { get; }
    private List<P> projects { get; }


    private SetOfPairs<S, P> studentsPreference { get; } = new();

    private SetOfPairs<L, S> lecturersPreference { get; } = new();
    private SetOfPairs<L, P> lecturerProjectPairs { get; } = new();

    private SetOfPairs<P, S> assigmentPairs { get; } = new();

    private Dictionary<L, int> lecturersCapacity { get; } = new();
    private Dictionary<P, int> projectsCapacity { get; } = new();

    public List<S> getListS()
    {
        return students;
    }

    public void assign(S s, P p)
    {
        assigmentPairs.Add(p, s);
    }

    public void breakAssigment(S s, P p)
    {
        assigmentPairs.Remove(p, s);
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

    public void setProjectCapacity(P p, int c)
    {
        projectsCapacity.Add(p, c);
    }

    public int getProjectCapacity(P p)
    {
        return projectsCapacity[p];
    }

    public void setProjects(L l, List<P> listP)
    {
        listP.ForEach(project => setProject(l, project));
    }


    public void setProject(L l, P p)
    {
        lecturerProjectPairs.Add(l, p);
    }

    public List<P> getProjects(L l)
    {
        return lecturerProjectPairs.getManyByKey(l);
    }

    public L getLecturerByProject(P p)
    {
        return lecturerProjectPairs.getByValue(p) ?? throw new InvalidOperationException();
    }


    public List<P> getStudentPreferences(S s)
    {
        return studentsPreference.getManyByKey(s);
    }

    public List<S> getLecturerPreferences(L l)
    {
        return lecturersPreference.getManyByKey(l);
    }

    public void setStudentPreferences(S s, List<P> pref)
    {
        foreach (var project in pref)
            setStudentPreferencePair(s, project);
    }

    public void setStudentPreferencePair(S s, P p)
    {
        studentsPreference.Add(s, p);
    }

    public void deleteStudentPreferencePair(S s, P p)
    {
        studentsPreference.Remove(s, p);
    }

    public void setLecturerPreferences(L s, List<S> pref)
    {
        foreach (var student in pref)
            setLecturerPreferencePair(s, student);
    }

    public List<S> getAssignedByP(P project)
    {
        return assigmentPairs.getManyByKey(project);
    }

    public List<S> getAssignedByL(L lecturer)
    {
        var lecturerProjects = lecturerProjectPairs.getManyByKey(lecturer);
        return assigmentPairs.pairs
            .FindAll(pair => lecturerProjects.Contains(pair.Item1))
            .Select(pair => pair.Item2).ToList();
    }

    public P? getProjectByAssigned(S s)
    {
        return assigmentPairs.getByValue(s);
    }

    public void setLecturerPreferencePair(L l, S s)
    {
        lecturersPreference.Add(l, s);
    }

    public void deleteFromStudentPref(S s, P p)
    {
        studentsPreference.Remove(s, p);
    }

    //TODO исключить этот метод
    public List<(L, List<(P, List<S>)>)> calcFinalAllocation()
    {
        var result = new List<(L, List<(P, List<S>)>)>();
        foreach (var lecturer in lecturers)
        {
            List<(P, List<S>)> projectStudentPairTuples = new List<(P, List<S>)>();
            var projects = getProjects(lecturer);
            projects.ForEach(project =>
            {
                var assignedStudents = getAssignedByP(project);
                projectStudentPairTuples.Add((project, assignedStudents));
            });
            result.Add(new(lecturer, projectStudentPairTuples));
        }

        return result;
    }
}