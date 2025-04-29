using MatchingLibrary.v2.Allocated.impl;
using MatchingLibrary.v2.Allocation;

namespace MatchingLibrary.Tests.Utils;

public class TwoStepAllocationJsonData
{
    public List<ComplexToOneAllocated> students { get; set; } = new();

    public List<ComplexTwoStepAllocated> lecturers { get; set; } = new();

    public List<ComplexDependedAllocated> projects { get; set; } = new();

    public Dictionary<string, List<string>> lecturersToProjects { get; set; } = new();
    public Dictionary<string, List<string>> lecturersPreferencesToStudents { get; set; } = new();
    public Dictionary<string, List<string>> studentsPreferencesToProjects { get; set; } = new();

    public Dictionary<string, List<string>> projectAssignedToStudents { get; set; } = new();

    public ITwoStepAllocation ToAllocation()
    {
        ITwoStepAllocation allocation = new TwoStepAllocation();

        foreach (var (lecturerName, projectsNames) in lecturersToProjects)
        {
            var lecturer = lecturers.Find(allocated => allocated.name == lecturerName);
            foreach (var projectName in projectsNames)
            {
                var project = projects.Find(allocated => allocated.name == projectName);
                lecturer.GetProjects().Add(project);
                project.SetOwner(lecturer);
            }
        }

        foreach (var (lecturerName, studentsNames) in lecturersPreferencesToStudents)
        {
            var lecturer = lecturers.Find(allocated => allocated.name == lecturerName);
            foreach (var studentName in studentsNames)
            {
                var student = students.Find(allocated => allocated.name == studentName);
                lecturer.GetPreferences().Add(student);
            }
        }

        foreach (var (studentName, projectsNames) in studentsPreferencesToProjects)
        {
            var student = students.Find(allocated => allocated.name == studentName);
            foreach (var projectName in projectsNames)
            {
                var project = projects.Find(allocated => allocated.name == projectName);
                student.GetPreferences().Add(project);
            }
        }

        foreach (var (projectName, studentsNames) in projectAssignedToStudents)
        {
            var project = projects.Find(allocated => allocated.name == projectName);

            foreach (var studentName in studentsNames)
            {
                var student = students.Find(allocated => allocated.name == studentName);
                student.Assign(project);
                project.Assign(student);
            }
        }

        allocation.GetStudents().AddRange(students);
        allocation.GetLecturers().AddRange(lecturers);
        allocation.GetProjects().AddRange(projects);
        return allocation;
    }
}