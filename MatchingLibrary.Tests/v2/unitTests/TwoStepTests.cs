using Common.Utils;
using MatchingLibrary.Tests.Utils;
using MatchingLibrary.v2.Allocated;
using MatchingLibrary.v2.Allocated.impl;
using MatchingLibrary.v2.Allocators;
using NUnit.Framework;

namespace MatchingLibrary.Tests.v2.unitTests;

public class TwoStepTests
{
    private const string TestRecourceFolder = "v2/resources/twostep/";
    private static readonly TwoStepAllocator _allocator = new();


    //[Test]
    public void CreateJson()
    {
        var students = new List<IToOneAllocated>
        {
            new ComplexToOneAllocated("s1"),
            new ComplexToOneAllocated("s2"),
            new ComplexToOneAllocated("s3")
        };
        var lecturers = new List<ITwoStepAllocated>
        {
            new ComplexTwoStepAllocated("L1", 2),
            new ComplexTwoStepAllocated("L2", 2)
        };
        var projects = new List<IDependentAllocated>
        {
            new ComplexDependedAllocated("p1", 1),
            new ComplexDependedAllocated("p2", 1),
            new ComplexDependedAllocated("p3", 1)
        };

        var data = new TwoStepAllocationJsonData
        {
            projects = projects.Cast<ComplexDependedAllocated>().ToList(),
            lecturers = lecturers.Cast<ComplexTwoStepAllocated>().ToList(),
            students = students.Cast<ComplexToOneAllocated>().ToList()
        };

        data.lecturersToProjects.Add("L1", new List<string> { "p1", "p2" });
        data.lecturersToProjects.Add("L2", new List<string> { "p3" });

        data.lecturersPreferencesToStudents.Add("L1", new List<string> { "s1", "s3", "s2" });
        data.lecturersPreferencesToStudents.Add("L2", new List<string> { "s2", "s1", "s3" });

        data.studentsPreferencesToProjects.Add("s1", new List<string> { "p1" });
        data.studentsPreferencesToProjects.Add("s2", new List<string> { "p2" });
        data.studentsPreferencesToProjects.Add("s3", new List<string> { "p3" });

        data.projectAssignedToStudents.Add("p1", new List<string> { "s1" });
        data.projectAssignedToStudents.Add("p2", new List<string> { "s2" });
        data.projectAssignedToStudents.Add("p3", new List<string> { "s3" });

        var json = JsonUtils.ToJson(data);
        IoUtils.WriteStringToFile(json, "resources/twostep/out.json");
    }

    [TestCase("/IsFinalTests/WhenNoPreferencesSet.json", true)]
    [TestCase("/IsFinalTests/WhenPairsArePossible.json", false)]
    [TestCase("/IsFinalTests/WhenPairsAreNotPossible.json", true)]
    [Test]
    public void TestIsFinal(string pathToAllocationJson, bool expectedResult)
    {
        var json = IoUtils.ReadFileAsString(TestRecourceFolder + pathToAllocationJson);
        var data = JsonUtils.FromJson<TwoStepAllocationJsonData>(json);
        var allocation = data.ToAllocation();

        Assert.AreEqual(expectedResult, _allocator.isFinal(allocation));
    }

    //none should be accepted
    [TestCase("ComputeIterationTests/whenNoPreferencesSet")]
    //s1 should be rejected
    [TestCase("ComputeIterationTests/whenStudentIsNotAcceptable")]
    //s1 should be accepted
    [TestCase("ComputeIterationTests/whenAcceptStudentAndProjectQuotaNotFull")]
    //s2 should be rejected
    [TestCase("ComputeIterationTests/whenProjectQuotaIsFull_AndOneOverQuotaAndIsWorse")]
    //s1 should be replaced by s2 
    [TestCase("ComputeIterationTests/whenProjectQuotaIsFull_AndOneOverQuotaAndIsBetter")]
    //s3 should be rejected
    [TestCase("ComputeIterationTests/whenLecturerQuotaIsFull_AndOneOverQuotaAndIsWorse")]
    //S3 is more preferable then s2 , so pair (s2 p2) will be rejected
    [TestCase("ComputeIterationTests/whenLecturerQuotaIsFull_AndOneOverQuotaAndIsBetter")]
    //s3 should be rejected on p1 but replace s2 on p2, pair (s2 p2) will be rejected
    [TestCase("ComputeIterationTests/whenLecturerQuotaIsFull_AndOneOverQuotaAndIsWorseOnOneProjectButBetterOnOther")]
    [Test]
    public void TestComputeIteration(string testFolder)
    {
        var initPath = TestRecourceFolder + testFolder + "/init.json";
        var expectedPath = TestRecourceFolder + testFolder + "/expected.json";
        var resultPath = TestRecourceFolder + testFolder + "/actual.json";

        //читамем данные инициализации
        var json = IoUtils.ReadFileAsString(initPath);
        var data = JsonUtils.FromJson<TwoStepAllocationJsonData>(json);
        var allocation = data.ToAllocation();
        //выполняем итерацию
        _allocator.computeIteration(allocation);
        var result = TestUtilsV2.GetAllocationResult(allocation);
        //сохраняем результат для анализа
        var resultJson = JsonUtils.ToJson(result);
        IoUtils.WriteStringToFile(resultJson, resultPath);
        //сравниваем результат с ожидамым
        var expectedJson = IoUtils.ReadFileAsString(expectedPath);
        Assert.AreEqual(expectedJson, resultJson);
    }
}