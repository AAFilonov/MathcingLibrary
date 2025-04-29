using MatchingLibrary.v3;
using MatchingLibrary.v3.exampleClasses;

namespace MatchingLibrary.Tests.Utils;

public class TestUtilsV2Old
{
    public static ( List<Allocated<Student>>, List<Allocated<Tutor>>) ReadV3File(string path)
    {
        var allocatedStudents = new List<Allocated<Student>>();
        var allocatedTutors = new List<Allocated<Tutor>>();

        // Получаем путь к текущему каталогу
        var currentDirectory = Directory.GetCurrentDirectory();

        // Преобразуем относительный путь к файлу в абсолютный путь
        var filePath = Path.GetFullPath($"{currentDirectory}\\..\\..\\..\\{path}");
        var lines = File.ReadLines(filePath).ToList();
        for (var index = 0; index < lines.Count; index++)
        {
            var line = lines[index];
            if (line == "")
            {
                lines.RemoveRange(0, index + 1);
                break;
            }

            var parts = line.Split(':');
            var student = new Student(parts[0])
            {
                preferenceString = parts[1]
            };
            allocatedStudents.Add(student.GetAllocated());
        }

        foreach (var line in lines)
        {
            if (line == "") break;

            var parts = line.Split(':');
            var tutor = new Tutor(parts[0]);
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
            var tutorNames = allocatedStudent.data.preferenceString.Split(' ');
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

    public static List<(Tutor?, List<Student>)> GetAllocationResult(
        List<Allocated<Tutor>> allocatedTutors, List<Allocated<Student>> allocatedStudents)
    {
        List<(Tutor, List<Student>)> t1 = allocatedTutors.Select(t => (t.data, t.GetAssignedData<Student>())).ToList();
        //List<(Student,Tutor)>  t2 = allocatedStudents.Select(s => (s.data, s.GetAssigned<Tutor>())).ToList();
        var t2 = allocatedStudents.Select(s =>
            {
                var assigned = s.GetAssignedData<Tutor>();
                return (assigned.Any() ? assigned[0] : null, new List<Student> { s.data });
            }
        ).ToList();

        var pairs = t1.Where(tuple => tuple.Item2.Any()).ToList();
        var emptyTutors = t1.Where(tuple => !tuple.Item2.Any()).ToList();
        var emptyStudents = t2.Where(tuple => tuple.Item1 == null).ToList();
        pairs.AddRange(emptyTutors);
        pairs.AddRange(emptyStudents);
        return pairs;
    }
}