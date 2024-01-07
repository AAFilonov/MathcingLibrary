using MatchingLibrary.v3;
using MatchingLibrary.v3.exampleClasses;

namespace MatchingLibrary.Tests.Utils;

public class TestUtils
{
    public static ( List<Allocated<Student>>, List<Allocated<Tutor>>) readV3File(string path)
    {
        List<Allocated<Student>> allocatedStudents = new List<Allocated<Student>>();
        List<Allocated<Tutor>> allocatedTutors = new List<Allocated<Tutor>>();

        // Получаем путь к текущему каталогу
        var currentDirectory = Directory.GetCurrentDirectory();

        // Преобразуем относительный путь к файлу в абсолютный путь
        var filePath = Path.GetFullPath($"{currentDirectory}\\..\\..\\..\\{path}");
        var lines = File.ReadLines(filePath).ToList();
        for (var index = 0; index < lines.Count; index++)
        {
            string line = lines[index];
            if (line == "")
            {
                lines.RemoveRange(0, index + 1);
                break;
            }

            string[] parts = line.Split(':');
            Student student = new Student(parts[0]);
            student.preferenceString = parts[1];
            allocatedStudents.Add(student.GetAllocated());
        }

        foreach (string line in lines)
        {
            if (line == "")
            {
                break;
            }

            string[] parts = line.Split(':');
            Tutor tutor = new Tutor(parts[0]);
            var allocatedTutor = tutor.GetAllocated();
            var studentNames = parts[1].Split(' ');
            foreach (var studentName in studentNames)
            {
                if (studentName == "")
                    break;
                allocatedTutor.preferences.Add(allocatedStudents.Find(st => st.data.name.Equals(studentName)) ??
                                               throw new InvalidOperationException());
            }

            allocatedTutors.Add(allocatedTutor);
        }

        foreach (var allocatedStudent in allocatedStudents)
        {
            string[] tutorNames = allocatedStudent.data.preferenceString.Split(' ');
            foreach (var tutorName in tutorNames)
            {
                if (tutorName == "")
                    break;
                allocatedStudent.preferences.Add(allocatedTutors.Find(st => st.data.name.Equals(tutorName)) ??
                                                 throw new InvalidOperationException());
            }
        }

        return (allocatedStudents, allocatedTutors);
    }

    public static   List<(Tutor?, List<Student>)>GetAllocationResult(
        List<Allocated<Tutor>> allocatedTutors,   List<Allocated<Student>> allocatedStudents )
    {
        List<(Tutor, List<Student>)> t1 = allocatedTutors.Select(t => (t.data, t.GetAssignedData<Student>())).ToList();
        //List<(Student,Tutor)>  t2 = allocatedStudents.Select(s => (s.data, s.GetAssigned<Tutor>())).ToList();
        List<(Tutor?, List<Student>)> t2 = allocatedStudents.Select(s =>
            {
                var assigned = s.GetAssignedData<Tutor>();
                return (assigned.Any() ? assigned[0] : null, new List<Student>() {s.data});
            }
        ).ToList();

        var pairs = t1.Where(tuple =>tuple.Item2.Any()).ToList();
        var emptyTutors = t1.Where(tuple => ! tuple.Item2.Any()).ToList();
        var emptyStudents = t2.Where(tuple =>tuple.Item1 == null).ToList();
        pairs.AddRange(emptyTutors);
        pairs.AddRange(emptyStudents);
        return pairs;
    }
}