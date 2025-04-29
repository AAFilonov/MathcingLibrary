using System;
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
        var tuple = TestUtilsV2Old.ReadV3File("resouces\\testHr_whenQuotaIsFull.txt");
        var allocatedStudents = tuple.Item1;
        var allocatedTutors = tuple.Item2;

        var arg = new HrAlgorithm<Tutor, Student>();
        arg.computeIteration(allocatedTutors, allocatedStudents);
        var pairs = TestUtilsV2Old.GetAllocationResult(allocatedTutors, allocatedStudents);
        var resultString = PrintUtilsV3.ToString(pairs);

        Console.WriteLine(resultString);
        //Should be Aab
        Assert.AreEqual("A:ab, ", resultString);
    }

    [Test]
    public void testHr_whenQuotaIsFull_AndOverQuotaIsBetter()
    {
        var tuple = TestUtilsV2Old.ReadV3File("resouces\\testHr_whenQuotaIsFull_AndOverQuotaIsBetter.txt");
        var allocatedStudents = tuple.Item1;
        var allocatedTutors = tuple.Item2;

        var arg = new HrAlgorithm<Tutor, Student>();
        arg.computeIteration(allocatedTutors, allocatedStudents);
        var pairs = TestUtilsV2Old.GetAllocationResult(allocatedTutors, allocatedStudents);
        var resultString = PrintUtilsV3.ToString(pairs);

        Console.WriteLine(resultString);
        //Should be Aab
        Assert.AreEqual("A:bc, :a, ", resultString);
    }

    [Test]
    public void testHr_whenQuotaIsFull_AndOverQuotaIsWorse()
    {
        var tuple = TestUtilsV2Old.ReadV3File("resouces\\testHr_whenQuotaIsFull_AndOverQuotaIsWorse.txt");
        var allocatedStudents = tuple.Item1;
        var allocatedTutors = tuple.Item2;

        var arg = new HrAlgorithm<Tutor, Student>();
        arg.computeIteration(allocatedTutors, allocatedStudents);
        var pairs = TestUtilsV2Old.GetAllocationResult(allocatedTutors, allocatedStudents);
        var resultString = PrintUtilsV3.ToString(pairs);

        Console.WriteLine(resultString);
        //Should be Aab
        Assert.AreEqual("A:ab, :c, ", resultString);
    }
}