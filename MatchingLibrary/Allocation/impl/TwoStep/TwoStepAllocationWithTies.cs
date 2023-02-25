using MatchingLibrary.Allocation.interfaces;
using MatchingLibrary.Utils;

namespace MatchingLibrary.Allocation.impl.TwoStep;

public class TwoStepAllocationWithTies<S, L, P>     :ITwoStepAllocationWithTies<S,L,P>
    where S : class
    where L : class
    where P : class
{
    public TwoStepAllocationWithTies(List<S> students, List<L> lecturers, List<P> projects)
    {
        this.students = students;
        this.lecturers = lecturers;
        this.projects = projects;
    }

    //наблюдение - преференсы преподователе это упорядочивание множества всех студентов, считающих их /их проект примелемым        
    private List<S> students { get; set; }
    private List<L> lecturers { get; set; }
    private List<P> projects { get; set; }


    private SetOfPairs<S,  List<P>> studentsPreferencePairs { get; set; } = new();

    private SetOfPairs<L,  List<S>> lecturersPreferencePairs { get; set; } = new();
   
    private SetOfPairs<L, P> lecturerProjectPairs { get; set; } = new();

    private  SetOfPairs<P, S> assigmentPairs { get; set; } = new();

    private Dictionary<L, int> lecturersCapacity { get; set; } = new();
    private Dictionary<P, int> projectsCapacity { get; set; } = new();

    public List<S> getStudents()
    {
        return students;
    }
   public void setLecturerCapacity(L l,int c)
    {
        ExceptionUtils.checkPresense(l, lecturers);
        lecturersCapacity.Add(l,c);
    }
    
    public void seProjectCapacity(P p,int c)
    {
        ExceptionUtils.checkPresense(p, projects);
        projectsCapacity.Add(p,c);
    }

    public int getLecturerCapacity(L l)
    {
        return lecturersCapacity[l];
    }
    public int getProjectCapacity(P p)
    {
        return projectsCapacity[p];
    }

    public void setProjects(L lecturer, List<P> projects)
    {
        ExceptionUtils.checkPresense(lecturer, lecturers);
        projects.ForEach(project => setProject(lecturer, project));
    }

    public List<P> getProjects(L lecturer)
    {
        return lecturerProjectPairs.getManyByKey(lecturer);
    }

    public void setProject(L lecturer, P project)
    {
        ExceptionUtils.checkPresense(project,projects);
        ExceptionUtils.checkPresense(lecturer,lecturers);
        lecturerProjectPairs.Add(lecturer, project);
    }

    public L getLecturerByProject(P project)
    {
        return lecturerProjectPairs.getByValue(project) ?? throw new InvalidOperationException();
    }

    public void assign(S s, P p)
    {
        assigmentPairs.Add(p, s);
    }

    public void breakAssigment(S s, P p)
    {
        assigmentPairs.Remove(p, s);
    }

    
    public List<List<P>> getStudentPreferences(S student)
    {
        return studentsPreferencePairs.getManyByKey(student);
    }

    public  List<List<S>> getLecturerPreferences(L lecturer)
    {
        return lecturersPreferencePairs.getManyByKey(lecturer);
    }

    public void setStudentPreferences(S student, List< List<P>> pref)
    {
        foreach (var project in pref)
            setStudentPreferencePair(student, project);
    }

    public void setStudentPreferencePair(S student,  List<P> projectTie)
    {
        studentsPreferencePairs.Add(student, projectTie);
    }

   

    public void deleteStudentPreferencePair(S student,  List<P> projectTie)
    {
        studentsPreferencePairs.Remove(student, projectTie);
    }

    public void setLecturerPreferences(L lecturer, List<List<S>> studnetTies)
    {
        foreach (var tie in studnetTies)
            setLecturerPreferencePair(lecturer, tie);
    }

    public List<S> getAssigned(P p)
    {
        return assigmentPairs.getManyByKey(p);
    }

    public List<S> getAssigned(L l)
    {
        var lecturerProjects = lecturerProjectPairs.getManyByKey(l);
        return assigmentPairs.pairs
            .FindAll(pair => lecturerProjects.Contains(pair.Item1))
            .Select(pair => pair.Item2).ToList();
    }

    public P? getProjectByAssigned(S s)
    {
        return assigmentPairs.getByValue(s);
    }
    public void setLecturerPreferencePair(L lecturer, List<S> studentTie)
    {
        lecturersPreferencePairs.Add(lecturer, studentTie);
    }

    public void deleteFromStudentPref(S student, List<P> projectsTie)
    {
        studentsPreferencePairs.Remove(student, projectsTie);
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
                var assignedStudents = getAssigned(project);
                projectStudentPairTuples.Add((project, assignedStudents));
            });
            result.Add(new(lecturer, projectStudentPairTuples));
        }

        return result;
    }
    
}