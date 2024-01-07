using MatchingLibrary.Tests.Utils;
using MatchingLibrary.v1.Algorithms.impl;
using MatchingLibrary.v1.Allocated.Impl;
using MatchingLibrary.v1.Allocation.impl;
using MatchingLibrary.v2;
using MatchingLibrary.v2.Algorithms;
using MatchingLibrary.v2.Allocated;
using MatchingLibrary.v2.Allocated.impl;
using MatchingLibrary.v2.Allocation;
using NUnit.Framework;

namespace MatchingLibrary.Tests.v2;

[TestFixture]
public class HrTests
{
    private HrResidentAllocator alg;

    [SetUp]
    public void Setup()
    {
        alg = new HrResidentAllocator();
    }

    [TestFixture]
    public class IsFinalTests : HrTests
    {
        [Test]
        public void testIsFinal_WhenListsNotEmptyAndNoPairs()
        {
            List<IToOneAllocated> students = new List<IToOneAllocated>()
            {
                new ComplexToOneAllocated("a"),
                new ComplexToOneAllocated("b"),
                new ComplexToOneAllocated("c")
            };
            List<IToManyAllocated> lecturers = new List<IToManyAllocated>()
            {
                new ComplexToManyAllocated("A"),
                new ComplexToManyAllocated("B"),
                new ComplexToManyAllocated("C")
            };

            var allocation = new OneToManyAllocation(lecturers, students);

            Assert.AreEqual(true, alg.isFinal(allocation));
        }

        [Test]
        public void testIsFinal_WhenListsNotEmptyAndThereIsPreferences()
        {
            List<IToOneAllocated> students = new List<IToOneAllocated>()
            {
                new ComplexToOneAllocated("a"),
                new ComplexToOneAllocated("b"),
                new ComplexToOneAllocated("c")
            };
            List<IToManyAllocated> lecturers = new List<IToManyAllocated>()
            {
                new ComplexToManyAllocated("A"),
                new ComplexToManyAllocated("B"),
                new ComplexToManyAllocated("C")
            };

            var allocation = new OneToManyAllocation(lecturers, students);
            students[0].SetPreferences(new List<IAllocated>() { lecturers[0], lecturers[1] }); //A B
            
            Assert.AreEqual(false, alg.isFinal(allocation));
        }

        [Test]
        public void testIsFinal_WhenListsNotEmptyAndThereAndAllPaired()
        {
            List<IToOneAllocated> students = new List<IToOneAllocated>()
            {
                new ComplexToOneAllocated("a"),
                new ComplexToOneAllocated("b"),
                new ComplexToOneAllocated("c")
            };
            List<IToManyAllocated> lecturers = new List<IToManyAllocated>()
            {
                new ComplexToManyAllocated("A"),
                new ComplexToManyAllocated("B"),
                new ComplexToManyAllocated("C")
            };

            var allocation = new OneToManyAllocation(lecturers, students);
            students[0].SetPreferences(new List<IAllocated>() { lecturers[0] }); //A
            students[0].SetPreferences(new List<IAllocated>() { lecturers[1] }); //B 
            students[0].SetPreferences(new List<IAllocated>() { lecturers[2] }); //C 
            
            AllocationUtils.createAssignment(lecturers[0], students[0]);
            AllocationUtils.createAssignment(lecturers[1], students[1]);
            AllocationUtils.createAssignment(lecturers[2], students[2]);

            Assert.AreEqual(true, alg.isFinal(allocation));
        }
    }

    [TestFixture]
    public class ComputeIterationTests : HrTests
    {
        [Test]
        public void whenPreferencesOfStudentEmpty()
        {
            //A  ни к кому не обратится - будет не распределен
            
            List<IToOneAllocated> students = new List<IToOneAllocated>()
            {
                new ComplexToOneAllocated("a"),
            };
            List<IToManyAllocated> lecturers = new List<IToManyAllocated>()
            {
                new ComplexToManyAllocated("A"),
            };

            var allocation = new OneToManyAllocation(lecturers, students);
            students[0].SetPreferences(new List<IAllocated>() { }); // a:
            lecturers[0].SetPreferences(new List<IAllocated>() { }); // A:
            lecturers[0].SetCapacity(2);

            alg.computeIteration(allocation);

            var resultString = PrintUtilsV2.toString(allocation.GetAllocationResult());
            
            Console.WriteLine(resultString);
            // a should be rejected
            //Should be A_ _a
            Assert.AreEqual("[A:], [:a], ", resultString);
        
        }

        [Test]
        public void whenStundentIsNotAcceptable()
        {
            //Студент а обратиться но будет отвергнут 
            List<IToOneAllocated> students = new List<IToOneAllocated>()
            {
                new ComplexToOneAllocated("a"),
            };
            List<IToManyAllocated> lecturers = new List<IToManyAllocated>()
            {
                new ComplexToManyAllocated("A"),
            };

            var allocation = new OneToManyAllocation(lecturers, students);
            students[0].SetPreferences(new List<IAllocated>() { lecturers[0]}); // a: A
            lecturers[0].SetPreferences(new List<IAllocated>() { }); // A:
            lecturers[0].SetCapacity(2);

            alg.computeIteration(allocation);

            var resultString = PrintUtilsV2.toString(allocation.GetAllocationResult());
            
            Console.WriteLine(resultString);
          
            //Should be A_ _a
            Assert.AreEqual("[A:], [:a], ", resultString);
        }

        [Test]
        public void whenQuotaNotFull()
        {
            //Студент а обратиться и получит согласие
            List<IToOneAllocated> students = new List<IToOneAllocated>()
            {
                new ComplexToOneAllocated("a"),
            };
            List<IToManyAllocated> lecturers = new List<IToManyAllocated>()
            {
                new ComplexToManyAllocated("A"),
            };

            var allocation = new OneToManyAllocation(lecturers, students);
            students[0].SetPreferences(new List<IAllocated>() { lecturers[0]}); // a: A
            lecturers[0].SetPreferences(new List<IAllocated>() { students[0]}); // A: a
            lecturers[0].SetCapacity(2);

            alg.computeIteration(allocation);

            var resultString = PrintUtilsV2.toString(allocation.GetAllocationResult());
            
            Console.WriteLine(resultString);
            Assert.AreEqual("[A:a], ", resultString);
            //Should be Aa
        }

        [Test]
        public void whenQuotaIsFull()
        {
            //Студент а обратиться и получит согласие
            List<IToOneAllocated> students = new List<IToOneAllocated>()
            {
                new ComplexToOneAllocated("a"), new ComplexToOneAllocated("b")
            };
            List<IToManyAllocated> lecturers = new List<IToManyAllocated>()
            {
                new ComplexToManyAllocated("A"),
            };

            var allocation = new OneToManyAllocation(lecturers, students);
            students[0].SetPreferences(new List<IAllocated>() { lecturers[0]}); // a: A
            students[1].SetPreferences(new List<IAllocated>() { lecturers[0]}); // b: A
            lecturers[0].SetPreferences(new List<IAllocated>() { students[0], students[1]}); // A: a b
            lecturers[0].SetCapacity(2);

            alg.computeIteration(allocation);

            var resultString = PrintUtilsV2.toString(allocation.GetAllocationResult());
            
            Console.WriteLine(resultString);
            //Should be Aab
             Assert.AreEqual("[A:ab], ", resultString);
        }

        [Test]
        public void whenQuotaIsFull_AndOverQuotaIsWorse()
        {
            //Студент с обратиться и будет отвергнут так как не влезет в квоту
            List<IToOneAllocated> students = new List<IToOneAllocated>()
            {
                new ComplexToOneAllocated("a"), new ComplexToOneAllocated("b"), new ComplexToOneAllocated("c")
            };
            List<IToManyAllocated> lecturers = new List<IToManyAllocated>()
            {
                new ComplexToManyAllocated("A"),
            };

            var allocation = new OneToManyAllocation(lecturers, students);
            students[0].SetPreferences(new List<IAllocated>() { lecturers[0]}); // a: A
            students[1].SetPreferences(new List<IAllocated>() { lecturers[0]}); // b: A
            students[2].SetPreferences(new List<IAllocated>() { lecturers[0]}); // c: A
            lecturers[0].SetPreferences(new List<IAllocated>() { students[0], students[1], students[2]}); // A: a b c
            lecturers[0].SetCapacity(2);

            alg.computeIteration(allocation);
            var resultString = PrintUtilsV2.toString(allocation.GetAllocationResult());
            Console.WriteLine(resultString);
            //Should be Aab _c
            //c should be rejected
            Assert.AreEqual("[A:ab], [:c], ", resultString);
        }

        [Test]
        public void  whenQuotaIsFull_AndOneOverQuotaAndIsBetter()
        {
            //Студент с обратиться и будет отвергнут так как не влезет в квоту
            List<IToOneAllocated> students = new List<IToOneAllocated>()
            {
                new ComplexToOneAllocated("a"), new ComplexToOneAllocated("b"), new ComplexToOneAllocated("c")
            };
            List<IToManyAllocated> lecturers = new List<IToManyAllocated>()
            {
                new ComplexToManyAllocated("A"),
            };

            var allocation = new OneToManyAllocation(lecturers, students);
            students[0].SetPreferences(new List<IAllocated>() { lecturers[0]}); // a: A
            students[1].SetPreferences(new List<IAllocated>() { lecturers[0]}); // b: A
            students[2].SetPreferences(new List<IAllocated>() { lecturers[0]}); // c: A
            lecturers[0].SetPreferences(new List<IAllocated>() { students[2], students[0], students[1]}); // A: с a b
            lecturers[0].SetCapacity(2);
            
            alg.computeIteration(allocation);
            var resultString = PrintUtilsV2.toString(allocation.GetAllocationResult());
            Console.WriteLine(resultString);

            Console.WriteLine(resultString);
            //Should be Acb _b
            Assert.AreEqual("[A:ac], [:b], ", resultString);
        }
    }
}