using MatchingLibrary.Algorithms.impl;
using MatchingLibrary.Allocation.impl;
using MatchingLibrary.Allocation.impl.TwoStep;
using MatchingLibrary.Tests.Utils;
using MatchingLibrary.Utils;
using NUnit.Framework;

namespace MatchingLibrary.Tests.UntiTests.Algorithms;

[TestFixture]
public class SpTests
{
    private SpAlgorithm<
        SimpleAllocated,
        SimpleAllocated,
        SimpleAllocated> alg;

    [SetUp]
    public void Setup()
    {
        alg = new SpAlgorithm<SimpleAllocated, SimpleAllocated, SimpleAllocated>();
    }


    [TestFixture]
    public class IsFinalTests : SpTests
    {
        [Test]
        public void testIsFinal_WhenListsNotEmptyAndNoPairs()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
                new("s3")
            };
            List<SimpleAllocated> lecturers = new List<SimpleAllocated>()
            {
                new("l1"),
                new("l2")
            };
            List<SimpleAllocated> projects = new List<SimpleAllocated>()
            {
                new("p1"),
                new("p2"),
                new("p3")
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleAllocated, SimpleAllocated>(students, lecturers, projects);
            allocation.setProjects(lecturers[0], new List<SimpleAllocated>() { projects[0], projects[1], });
            allocation.setProjects(lecturers[1], new List<SimpleAllocated>() { projects[2], });

            Assert.AreEqual(true, alg.isFinal(allocation));
        }

        [Test]
        public void testIsFinal_WhenListsNotEmptyAndThereIsPreferences()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
                new("s3")
            };
            List<SimpleAllocated> lecturers = new List<SimpleAllocated>()
            {
                new("l1"),
                new("l2")
            };
            List<SimpleAllocated> projects = new List<SimpleAllocated>()
            {
                new("p1"),
                new("p2"),
                new("p3")
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleAllocated, SimpleAllocated>(students, lecturers, projects);
            allocation.setProjects(lecturers[0], new List<SimpleAllocated>() { projects[0], projects[1], });
            allocation.setProjects(lecturers[1], new List<SimpleAllocated>() { projects[2], });

            allocation.setStudentPreferences(students[0],
                new List<SimpleAllocated>() { projects[0], projects[2] }); //s1: p1,p3


            Assert.AreEqual(false, alg.isFinal(allocation));
        }

        [Test]
        public void testIsFinal_WhenListsNotEmptyAndThereAndAllPaired()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
                new("s3")
            };
            List<SimpleAllocated> lecturers = new List<SimpleAllocated>()
            {
                new("l1"),
                new("l2")
            };
            List<SimpleAllocated> projects = new List<SimpleAllocated>()
            {
                new("p1"),
                new("p2"),
                new("p3")
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleAllocated, SimpleAllocated>(students, lecturers, projects);
            allocation.setProjects(lecturers[0], new List<SimpleAllocated>() { projects[0], projects[1], });
            allocation.setProjects(lecturers[1], new List<SimpleAllocated>() { projects[2], });

            allocation.setStudentPreferences(students[0],
                new List<SimpleAllocated>() { projects[0] }); //s1: p1,p3
            allocation.setStudentPreferences(students[1],
                new List<SimpleAllocated>() { projects[1] }); //s1: p1,p3
            allocation.setStudentPreferences(students[2],
                new List<SimpleAllocated>() { projects[2] }); //s1: p1,p3

            allocation.assign(students[0], projects[0]);
            allocation.assign(students[1], projects[1]);
            allocation.assign(students[2], projects[2]);

            Assert.AreEqual(true, alg.isFinal(allocation));
        }
    }


    [TestFixture]
    public class ComputeIterationTests : SpTests
    {
        [Test]
        public void whenPreferencesOfStudentEmpty()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
            };
            List<SimpleAllocated> lecturers = new List<SimpleAllocated>()
            {
                new("l1"),
            };
            List<SimpleAllocated> projects = new List<SimpleAllocated>()
            {
                new("p1"),
                new("p2"),
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleAllocated, SimpleAllocated>(students, lecturers, projects);
            allocation.setProjects(lecturers[0], new List<SimpleAllocated>() { projects[0], projects[1], });
            
            allocation.setLecturerCapacity(lecturers[0], 2);
            
            allocation.setProjectCapacity(projects[0], 1);
            allocation.setProjectCapacity(projects[1], 2);
      
            
            alg.computeIteration(allocation);
            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());

            Assert.AreEqual("pair: [l1 { p1 ( )p2 ( )}], ", resultString);
            //should be empty - no reachable pairs 
        }

        [Test]
        public void whenStundentIsNotAcceptable()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
            };
            List<SimpleAllocated> lecturers = new List<SimpleAllocated>()
            {
                new("l1"),
            };
            List<SimpleAllocated> projects = new List<SimpleAllocated>()
            {
                new("p1"),
                new("p2"),
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleAllocated, SimpleAllocated>(students, lecturers, projects);
            allocation.setProjects(lecturers[0], new List<SimpleAllocated>() { projects[0], projects[1], });
  
            allocation.setLecturerCapacity(lecturers[0], 2);
            
            allocation.setProjectCapacity(projects[0], 1);
            allocation.setProjectCapacity(projects[1], 2);

            allocation.setStudentPreferences(students[0], new List<SimpleAllocated>() { projects[0] }); //s1 : p1
            allocation.setLecturerPreferences(lecturers[0], new List<SimpleAllocated>() { students[1] }); //l1 : s2

            alg.computeIteration(allocation);
            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            //should be empty - no reachable pairs 
            Assert.AreEqual("pair: [l1 { p1 ( )p2 ( )}], ", resultString);
        }

        [Test]
        public void whenProjectQuotaNotFull()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
            };
            List<SimpleAllocated> lecturers = new List<SimpleAllocated>()
            {
                new("l1"),
            };
            List<SimpleAllocated> projects = new List<SimpleAllocated>()
            {
                new("p1"),
                new("p2"),
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleAllocated, SimpleAllocated>(students, lecturers, projects);
            allocation.setProjects(lecturers[0], new List<SimpleAllocated>() { projects[0], projects[1], });

            allocation.setStudentPreferences(students[0], new List<SimpleAllocated>() { projects[0] }); //s1 : p1
            allocation.setLecturerPreferences(lecturers[0], new List<SimpleAllocated>() { students[0] }); //l1 : s1
  
            allocation.setLecturerCapacity(lecturers[0], 2);
            
            allocation.setProjectCapacity(projects[0], 2);
            allocation.setProjectCapacity(projects[1], 1);

            alg.computeIteration(allocation);
            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            //should be  l1{ p1:(s1) p2:()}
            Assert.AreEqual("pair: [l1 { p1 ( s1 )p2 ( )}], ", resultString);
        }

        [Test]
        public void whenProjectQuotaIsFull_AndOneOverQuotaAndIsWorse()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
            };
            List<SimpleAllocated> lecturers = new List<SimpleAllocated>()
            {
                new("l1"),
            };
            List<SimpleAllocated> projects = new List<SimpleAllocated>()
            {
                new("p1"),
                new("p2"),
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleAllocated, SimpleAllocated>(students, lecturers, projects);
            allocation.setProjects(lecturers[0],
                new List<SimpleAllocated>() { projects[0], projects[1], }); //l1 : p1 p2
            allocation.setStudentPreferences(students[0], new List<SimpleAllocated>() { projects[0] }); //s1 : p1
            allocation.setStudentPreferences(students[1], new List<SimpleAllocated>() { projects[0] }); //s2 : p1
            allocation.setLecturerPreferences(lecturers[0],
                new List<SimpleAllocated>() { students[0], students[1] }); //l1 : s1 s2

  
            allocation.setLecturerCapacity(lecturers[0], 2);
            
            allocation.setProjectCapacity(projects[0], 1);
            allocation.setProjectCapacity(projects[1], 1);

            alg.computeIteration(allocation);
            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());

            //s2 should be rejected
            //should be  l1{ p1:(s1) p2:()}
            Assert.AreEqual("pair: [l1 { p1 ( s1 )p2 ( )}], ", resultString);
        }

        [Test]
        public void whenProjectQuotaIsFull_AndOneOverQuotaAndIsBetter()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
            };
            List<SimpleAllocated> lecturers = new List<SimpleAllocated>()
            {
                new("l1"),
            };
            List<SimpleAllocated> projects = new List<SimpleAllocated>()
            {
                new("p1"),
                new("p2"),
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleAllocated, SimpleAllocated>(students, lecturers, projects);
            allocation.setProjects(lecturers[0],
                new List<SimpleAllocated>() { projects[0], projects[1], }); //l1 : p1 p2
            allocation.setStudentPreferences(students[0], new List<SimpleAllocated>() { projects[0] }); //s1 : p1
            allocation.setStudentPreferences(students[1], new List<SimpleAllocated>() { projects[0] }); //s2 : p1
            allocation.setLecturerPreferences(lecturers[0],
                new List<SimpleAllocated>() { students[1], students[0] }); //l1 : s2 s1

            allocation.setLecturerCapacity(lecturers[0], 2);
            
            allocation.setProjectCapacity(projects[0], 1);
            allocation.setProjectCapacity(projects[1], 1);

            alg.computeIteration(allocation);
            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());

            //s1 should be replaced by s2 
            //should be  l1{ p1:(s2) p2:()}
            Assert.AreEqual("pair: [l1 { p1 ( s2 )p2 ( )}], ", resultString);
        }

        [Test]
        public void whenLecturerQuotaIsFull_AndOneOverQuotaAndIsWorse()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
                new("s3"),
            };
            List<SimpleAllocated> lecturers = new List<SimpleAllocated>()
            {
                new("l1"),
            };
            List<SimpleAllocated> projects = new List<SimpleAllocated>()
            {
                new("p1"),
                new("p2"),
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleAllocated, SimpleAllocated>(students, lecturers, projects);
            allocation.setProjects(lecturers[0],
                new List<SimpleAllocated>() { projects[0], projects[1], }); //l1 : p1 p2
            allocation.setStudentPreferences(students[0], new List<SimpleAllocated>() { projects[0] }); //s1 : p1
            allocation.setStudentPreferences(students[1], new List<SimpleAllocated>() { projects[1] }); //s2 : p2
            allocation.setStudentPreferences(students[2], new List<SimpleAllocated>() { projects[0] }); //s3 : p1
            allocation.setLecturerPreferences(lecturers[0],
                new List<SimpleAllocated>() { students[0], students[1], students[2] }); //l1 : s1 s2 s3

            allocation.setLecturerCapacity(lecturers[0], 2);
            
            allocation.setProjectCapacity(projects[0], 2);
            allocation.setProjectCapacity(projects[1], 1);
            alg.computeIteration(allocation);
            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());

            //s3 should be rejected
            //should be  l1{ p1:(s1) p2:(s2)}
            Assert.AreEqual("pair: [l1 { p1 ( s1 )p2 ( s2 )}], ", resultString);
        }

        [Test]
        public void whenLecturerQuotaIsFull_AndOneOverQuotaAndIsBetter()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
                new("s3"),
            };
            List<SimpleAllocated> lecturers = new List<SimpleAllocated>()
            {
                new("l1"),
            };
            List<SimpleAllocated> projects = new List<SimpleAllocated>()
            {
                new("p1"),
                new("p2"),
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleAllocated, SimpleAllocated>(students, lecturers, projects);
            allocation.setProjects(lecturers[0],
                new List<SimpleAllocated>() { projects[0], projects[1], }); //l1 : p1 p2
            allocation.setStudentPreferences(students[0], new List<SimpleAllocated>() { projects[0] }); //s1 : p1
            allocation.setStudentPreferences(students[1], new List<SimpleAllocated>() { projects[1] }); //s2 : p2
            allocation.setStudentPreferences(students[2], new List<SimpleAllocated>() { projects[0] }); //s3 : p1
            allocation.setLecturerPreferences(lecturers[0],
                new List<SimpleAllocated>() { students[2], students[0], students[1] }); //l1 :s3 s1 s2 

            allocation.setLecturerCapacity(lecturers[0], 2);
            
            allocation.setProjectCapacity(projects[0], 2);
            allocation.setProjectCapacity(projects[1], 1);
            
            alg.computeIteration(allocation);
            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());

            //S3 is more preferable then s2 , so pair s2 p2 will be rejected
            Assert.AreEqual("pair: [l1 { p1 ( s1 s3 )p2 ( )}], ", resultString);
        }

        [Test]
        public void whenLecturerQuotaIsFull_AndOneOverQuotaAndIsWorseOnOneProjectAndBetterOnOther()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
                new("s3"),
            };
            List<SimpleAllocated> lecturers = new List<SimpleAllocated>()
            {
                new("l1"),
            };
            List<SimpleAllocated> projects = new List<SimpleAllocated>()
            {
                new("p1"),
                new("p2"),
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleAllocated, SimpleAllocated>(students, lecturers, projects);
            allocation.setProjects(lecturers[0],
                new List<SimpleAllocated>() { projects[0], projects[1], }); //l1 : p1 p2
            allocation.setStudentPreferences(students[0], new List<SimpleAllocated>() { projects[0] }); //s1 : p1
            allocation.setStudentPreferences(students[1], new List<SimpleAllocated>() { projects[1] }); //s2 : p2
            allocation.setStudentPreferences(students[2],
                new List<SimpleAllocated>() { projects[0], projects[1] }); //s3 : p1 p2
            allocation.setLecturerPreferences(lecturers[0],
                new List<SimpleAllocated>() { students[0], students[2], students[1] }); //l1 : s1 s3 s2 
            
            allocation.setLecturerCapacity(lecturers[0], 2);
            
            allocation.setProjectCapacity(projects[0], 1);
            allocation.setProjectCapacity(projects[1], 2);

            alg.computeIteration(allocation);
            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());

            //s3 should be rejected  on p1 but replace s2 on p2
            Assert.AreEqual("pair: [l1 { p1 ( s1 )p2 ( s3 )}], ", resultString);


            SetOfPairs<SimpleAllocated, SimpleAllocated> pairs = new();
            pairs.Add(projects[0],students[0]);
       
        }
    }
}