using System.Collections.Generic;
using Common.Utils;
using MatchingLibrary.v2.Allocated;
using MatchingLibrary.v2.Allocated.impl;
using MatchingLibrary.v2.Allocation;
using MatchingLibrary.v2.Allocators;
using NUnit.Framework;
using HrResidentAllocator = MatchingLibrary.v2.Allocators.HrResidentAllocator;
using IToOneAllocated = MatchingLibrary.v2.Allocated.IToOneAllocated;

namespace MatchingLibrary.Tests.v2.unitTests;

[TestFixture]
public class HrFloatTests
{
    private const string TestRecourceFolder = "resources/HrFloat/";
    private HrResidentAllocator _alg = new();


    [TestFixture]
    public class AppTest : HrTests
    {
        public void FillPreferences(IToOneAllocated subordinate, List<IToManyFloatAllocated> leaders)
        {
            //Заполнить препдочтения 
        }

        public void FillPreferences(IToManyFloatAllocated leader, List<IToOneAllocated> subordinates)
        {
            //Заполнить препдочтения 
        }

        [Test]
        public void Example_1()
        {
            var m = new List<IToOneAllocated>
            {
                new ComplexToOneAllocated("m1"),
                new ComplexToOneAllocated("m2"),
                new ComplexToOneAllocated("m3"),
                new ComplexToOneAllocated("m4"),
                new ComplexToOneAllocated("m5"),
                new ComplexToOneAllocated("m6"),
                new ComplexToOneAllocated("m7"),
                new ComplexToOneAllocated("m8"),
                new ComplexToOneAllocated("m9"),
                new ComplexToOneAllocated("m10"),
                new ComplexToOneAllocated("m11")
            };
            var n = new List<IToManyFloatAllocated>
            {
                new BaseToManyFloatAllocated("n3", 4.0),
                new BaseToManyFloatAllocated("n4", 4.0),
                new BaseToManyFloatAllocated("n5", 4.0)
            };

            var allocation = new OneToManyFloatAllocation(n, m);
            foreach (var subordinate in m) FillPreferences(subordinate, n);
            foreach (var leader in n) FillPreferences(leader, m);

            n[3].Assign(m[6]);
            n[4].Assign(m[7]);
            n[4].Assign(m[8]);
            n[5].Assign(m[9]);
            n[5].Assign(m[10]);
            n[5].Assign(m[11]);


            var testPath = TestRecourceFolder + "Example_1";

            string actualJson, expectedJson;
            var alg = new HrResidentFloatAllocator();
            //Step 1
            alg.computeStep(m[1], allocation);
            actualJson = JsonUtils.ToJson(allocation.GetAllocationResult());
            expectedJson = IoUtils.ReadFileAsString(testPath + "/step1_expected.json");
            Assert.AreEqual(expectedJson, actualJson);

            //Step 2
            alg.computeStep(m[2], allocation);
            actualJson = JsonUtils.ToJson(allocation.GetAllocationResult());
            expectedJson = IoUtils.ReadFileAsString(testPath + "/step2_expected.json");
            Assert.AreEqual(expectedJson, actualJson);

            //Step 3
            alg.computeStep(m[3], allocation);
            actualJson = JsonUtils.ToJson(allocation.GetAllocationResult());
            expectedJson = IoUtils.ReadFileAsString(testPath + "/step3_expected.json");
            Assert.AreEqual(expectedJson, actualJson);
            //Step 4
            alg.computeStep(m[4], allocation);
            actualJson = JsonUtils.ToJson(allocation.GetAllocationResult());
            expectedJson = IoUtils.ReadFileAsString(testPath + "/step4_expected.json");
            Assert.AreEqual(expectedJson, actualJson);

            //Step 5
            alg.computeStep(m[5], allocation);
            actualJson = JsonUtils.ToJson(allocation.GetAllocationResult());
            expectedJson = IoUtils.ReadFileAsString(testPath + "/step5_expected.json");
            Assert.AreEqual(expectedJson, actualJson);
        }
    }
}