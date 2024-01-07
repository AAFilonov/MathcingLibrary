using MatchingLibrary.Tests.Utils;
using MatchingLibrary.v3;
using MatchingLibrary.v3.exampleClasses;
using NUnit.Framework;

namespace MatchingLibrary.Tests.v3;

[TestFixture]
public class Test
{
   

    [Test]
    public void testHr_whenQuotaIsFull()
    {
        var tuple = TestUtils.readV3File("resouces\\testHr_whenQuotaIsFull.txt");
        List<Allocated<Student>> allocatedStudents = tuple.Item1;
        List<Allocated<Tutor>> allocatedTutors = tuple.Item2;

        var arg = new HrAlgorithm<Tutor, Student>();
        arg.computeIteration(allocatedTutors, allocatedStudents);
        var pairs = TestUtils.GetAllocationResult(allocatedTutors, allocatedStudents);
        string resultString = PrintUtilsV3.toString(pairs);

        Console.WriteLine(resultString);
        //Should be Aab
        Assert.AreEqual("A:ab, ", resultString);
    }
    
    [Test]
    public void testHr_whenQuotaIsFull_AndOverQuotaIsBetter()
    {
        var tuple = TestUtils.readV3File("resouces\\testHr_whenQuotaIsFull_AndOverQuotaIsBetter.txt");
        List<Allocated<Student>> allocatedStudents = tuple.Item1;
        List<Allocated<Tutor>> allocatedTutors = tuple.Item2;

        var arg = new HrAlgorithm<Tutor, Student>();
        arg.computeIteration(allocatedTutors, allocatedStudents);
        var pairs = TestUtils.GetAllocationResult(allocatedTutors, allocatedStudents);
        string resultString = PrintUtilsV3.toString(pairs);

        Console.WriteLine(resultString);
        //Should be Aab
        Assert.AreEqual("A:bc, :a, ", resultString);
    } 
    
    [Test]
    public void testHr_whenQuotaIsFull_AndOverQuotaIsWorse()
    {
        var tuple = TestUtils.readV3File("resouces\\testHr_whenQuotaIsFull_AndOverQuotaIsWorse.txt");
        List<Allocated<Student>> allocatedStudents = tuple.Item1;
        List<Allocated<Tutor>> allocatedTutors = tuple.Item2;

        var arg = new HrAlgorithm<Tutor, Student>();
        arg.computeIteration(allocatedTutors, allocatedStudents);
        var pairs = TestUtils.GetAllocationResult(allocatedTutors, allocatedStudents);
        string resultString = PrintUtilsV3.toString(pairs);

        Console.WriteLine(resultString);
        //Should be Aab
        Assert.AreEqual("A:ab, :c, ", resultString);
    }
}