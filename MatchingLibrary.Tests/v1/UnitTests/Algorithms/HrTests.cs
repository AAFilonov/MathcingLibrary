using MatchingLibrary.Tests.Utils;
using MatchingLibrary.v1.Algorithms.impl;
using MatchingLibrary.v1.Allocated.Impl;
using MatchingLibrary.v1.Allocation.impl;
using NUnit.Framework;

namespace MatchingLibrary.Tests.v1.UnitTests.Algorithms;

[TestFixture]
public class HrTests
{
    private HrAlgorithm<NamedAllocated, NamedAllocated> alg;

    [SetUp]
    public void Setup()
    {
        alg = new HrAlgorithm<NamedAllocated, NamedAllocated>();
    }

    [TestFixture]
    public class IsFinalTests : HrTests
    {
        [Test]
        public void testIsFinal_WhenListsNotEmptyAndNoPairs()
        {
            List<NamedAllocated> students = new List<NamedAllocated>()
            {
                new("a"),
                new("b"),
                new("c")
            };
            List<NamedAllocated> lecturers = new List<NamedAllocated>()
            {
                new("A"),
                new("B"),
                new("C")
            };

            var allocation = new OneToManyAllocation<NamedAllocated, NamedAllocated>(students, lecturers);

            Assert.AreEqual(true, alg.isFinal(allocation));
        }

        [Test]
        public void testIsFinal_WhenListsNotEmptyAndThereIsPreferences()
        {
            List<NamedAllocated> students = new List<NamedAllocated>()
            {
                new("a"),
                new("b"),
                new("c")
            };
            List<NamedAllocated> lecturers = new List<NamedAllocated>()
            {
                new("A"),
                new("B"),
                new("C")
            };

            var allocation = new OneToManyAllocation<NamedAllocated, NamedAllocated>(students, lecturers);
            allocation.setStudentPreferences(students[0],
                new List<NamedAllocated>() { lecturers[0], lecturers[1] }); //A B

            Assert.AreEqual(false, alg.isFinal(allocation));
        }

        [Test]
        public void testIsFinal_WhenListsNotEmptyAndThereAndAllPaired()
        {
            List<NamedAllocated> students = new List<NamedAllocated>()
            {
                new("a"),
                new("b"),
                new("c")
            };
            List<NamedAllocated> lecturers = new List<NamedAllocated>()
            {
                new("A"),
                new("B"),
                new("C")
            };

            var allocation = new OneToManyAllocation<NamedAllocated, NamedAllocated>(students, lecturers);
            allocation.setStudentPreferences(students[0], new List<NamedAllocated>() { lecturers[0] }); //A
            allocation.setStudentPreferences(students[0], new List<NamedAllocated>() { lecturers[1] }); //B
            allocation.setStudentPreferences(students[0], new List<NamedAllocated>() { lecturers[2] }); //C
            allocation.assign(lecturers[0], students[0]);
            allocation.assign(lecturers[1], students[1]);
            allocation.assign(lecturers[2], students[2]);

            Assert.AreEqual(true, alg.isFinal(allocation));
        }
    }

    [TestFixture]
    public class ComputeIterationTests : HrTests
    {
        [Test]
        public void whenPreferencesOfStudentEmpty()
        {
            List<NamedAllocated> students = new List<NamedAllocated>()
                { new("a") };
            List<NamedAllocated> lecturers = new List<NamedAllocated>()
                { new("A"), };

            //A  ни к кому не обратится - будет не распределен

            var allocation = new OneToManyAllocation<NamedAllocated, NamedAllocated>(students, lecturers);
            allocation.setStudentPreferences(students[0], new List<NamedAllocated>() { }); // a: 

            allocation.setLecturerPreferences(lecturers[0], new List<NamedAllocated>() { }); //A:
            allocation.setLecturerCapacity(lecturers[0],2);
            
            alg.computeIteration(allocation);

            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            
            Console.WriteLine(resultString);
            // a should be rejected
            //Should be A_
            Assert.AreEqual("pair: [A { }], ", resultString);
        
        }

        [Test]
        public void whenStundentIsNotAcceptable()
        {
            List<NamedAllocated> students = new List<NamedAllocated>()
                { new("a") };
            List<NamedAllocated> lecturers = new List<NamedAllocated>()
                { new("A"), };

            var allocation = new OneToManyAllocation<NamedAllocated, NamedAllocated>(students, lecturers);
            allocation.setStudentPreferences(students[0],
                new List<NamedAllocated>() { lecturers[0] }); // a: 

            allocation.setLecturerPreferences(lecturers[0], new List<NamedAllocated>() { }); //A:
            allocation.setLecturerCapacity(lecturers[0],2);

            alg.computeIteration(allocation);

            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            Console.WriteLine(resultString);
       
            // a should be rejected
            //Should be A_
            Assert.AreEqual("pair: [A { }], ", resultString);
        }

        [Test]
        public void whenQuotaNotFull()
        {
            List<NamedAllocated> students = new List<NamedAllocated>()
                { new("a") };
            List<NamedAllocated> lecturers = new List<NamedAllocated>()
                { new("A"), };

            var allocation = new OneToManyAllocation<NamedAllocated, NamedAllocated>(students, lecturers);
            allocation.setStudentPreferences(students[0],
                new List<NamedAllocated>() { lecturers[0] }); //a: A 

            allocation.setLecturerPreferences(lecturers[0], new List<NamedAllocated>() { students[0] }); //A: a
            allocation.setLecturerCapacity(lecturers[0],2);
            alg.computeIteration(allocation);

            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            Console.WriteLine(resultString);
            Assert.AreEqual("pair: [A { a }], ", resultString);
            //Should be Aa
        }

        [Test]
        public void whenQuotaIsFull()
        {
            List<NamedAllocated> students = new List<NamedAllocated>()
                { new("a"), new("b") };
            List<NamedAllocated> lecturers = new List<NamedAllocated>()
                { new("A"), };

            var allocation = new OneToManyAllocation<NamedAllocated, NamedAllocated>(students, lecturers);
            allocation.setStudentPreferences(students[0],
                new List<NamedAllocated>() { lecturers[0] }); // a: A
            allocation.setStudentPreferences(students[1],
                new List<NamedAllocated>() { lecturers[0] }); // b: A

          
            allocation.setLecturerPreferences(lecturers[0],
                new List<NamedAllocated>() { students[0], students[1] }); //A: a b
            allocation.setLecturerCapacity(lecturers[0],2);
            alg.computeIteration(allocation);

            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            Console.WriteLine(resultString);
            Assert.AreEqual("pair: [A { a b }], ", resultString);
            //Should be Aab
        }

        [Test]
        public void whenQuotaIsFull_AndOneOverQuotaAndIsWorse()
        {
            List<NamedAllocated> students = new List<NamedAllocated>()
                { new("a"), new("b"), new("c") };
            List<NamedAllocated> lecturers = new List<NamedAllocated>()
                { new("A"), };

            var allocation = new OneToManyAllocation<NamedAllocated, NamedAllocated>(students, lecturers);
            allocation.setStudentPreferences(students[0],
                new List<NamedAllocated>() { lecturers[0] }); // a: A
            allocation.setStudentPreferences(students[1],
                new List<NamedAllocated>() { lecturers[0] }); // b: A
            allocation.setStudentPreferences(students[2],
                new List<NamedAllocated>() { lecturers[0] }); // c: A

            allocation.setLecturerPreferences(lecturers[0],
                new List<NamedAllocated>() { students[0], students[1], students[2] }); //A: a b c
            allocation.setLecturerCapacity(lecturers[0],2);
            
            alg.computeIteration(allocation);

            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            Console.WriteLine(resultString);
            Assert.AreEqual("pair: [A { a b }], ", resultString);
            //c should be rejected
            //Should be Aab 
        }

        [Test]
        public void  whenQuotaIsFull_AndOneOverQuotaAndIsBetter()
        {
            List<NamedAllocated> students = new List<NamedAllocated>()
                { new("a"), new("b"), new("c") };
            List<NamedAllocated> lecturers = new List<NamedAllocated>()
                { new("A"), };

            var allocation = new OneToManyAllocation<NamedAllocated, NamedAllocated>(students, lecturers);
            allocation.setStudentPreferences(students[0],
                new List<NamedAllocated>() { lecturers[0] }); // a: A
            allocation.setStudentPreferences(students[1],
                new List<NamedAllocated>() { lecturers[0] }); // b: A
            allocation.setStudentPreferences(students[2],
                new List<NamedAllocated>() { lecturers[0] }); // c: A

            allocation.setLecturerPreferences(lecturers[0],
                new List<NamedAllocated>() { students[2], students[0], students[1] }); //A: c a b

            allocation.setLecturerCapacity(lecturers[0],2);
            alg.computeIteration(allocation);

            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            Console.WriteLine(resultString);
            Assert.AreEqual("pair: [A { a c }], ", resultString);
            //Should be Acb _b
        }
    }
}