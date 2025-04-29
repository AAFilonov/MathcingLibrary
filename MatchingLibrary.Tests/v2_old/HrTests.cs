using System;
using System.Collections.Generic;
using MatchingLibrary.Tests.Utils;
using MatchingLibrary.v2_OLD;
using MatchingLibrary.v2_OLD.Allocated;
using MatchingLibrary.v2_OLD.Allocated.impl;
using MatchingLibrary.v2_OLD.Allocation;
using MatchingLibrary.v2_OLD.Allocators;
using NUnit.Framework;

namespace MatchingLibrary.Tests.v2_old;

[TestFixture]
public class HrTests
{
    [SetUp]
    public void Setup()
    {
        _alg = new HrResidentAllocator();
    }

    private HrResidentAllocator _alg;

    [TestFixture]
    public class IsFinalTests : HrTests
    {
        [Test]
        public void testIsFinal_WhenListsNotEmptyAndNoPairs()
        {
            var students = new List<IToOneAllocated>
            {
                new ComplexToOneAllocated("a"),
                new ComplexToOneAllocated("b"),
                new ComplexToOneAllocated("c")
            };
            var lecturers = new List<IToManyAllocated>
            {
                new ComplexToManyAllocated("A"),
                new ComplexToManyAllocated("B"),
                new ComplexToManyAllocated("C")
            };

            var allocation = new OneToManyAllocation(lecturers, students);

            Assert.AreEqual(true, _alg.isFinal(allocation));
        }

        [Test]
        public void testIsFinal_WhenListsNotEmptyAndThereIsPreferences()
        {
            var students = new List<IToOneAllocated>
            {
                new ComplexToOneAllocated("a"),
                new ComplexToOneAllocated("b"),
                new ComplexToOneAllocated("c")
            };
            var lecturers = new List<IToManyAllocated>
            {
                new ComplexToManyAllocated("A"),
                new ComplexToManyAllocated("B"),
                new ComplexToManyAllocated("C")
            };

            var allocation = new OneToManyAllocation(lecturers, students);
            students[0].SetPreferences(new List<IAllocated> { lecturers[0], lecturers[1] }); //A B

            Assert.AreEqual(false, _alg.isFinal(allocation));
        }

        [Test]
        public void testIsFinal_WhenListsNotEmptyAndThereAndAllPaired()
        {
            var students = new List<IToOneAllocated>
            {
                new ComplexToOneAllocated("a"),
                new ComplexToOneAllocated("b"),
                new ComplexToOneAllocated("c")
            };
            var lecturers = new List<IToManyAllocated>
            {
                new ComplexToManyAllocated("A"),
                new ComplexToManyAllocated("B"),
                new ComplexToManyAllocated("C")
            };

            var allocation = new OneToManyAllocation(lecturers, students);
            students[0].SetPreferences(new List<IAllocated> { lecturers[0] }); //A
            students[0].SetPreferences(new List<IAllocated> { lecturers[1] }); //B 
            students[0].SetPreferences(new List<IAllocated> { lecturers[2] }); //C 

            AllocationUtils.createAssignment(lecturers[0], students[0]);
            AllocationUtils.createAssignment(lecturers[1], students[1]);
            AllocationUtils.createAssignment(lecturers[2], students[2]);

            Assert.AreEqual(true, _alg.isFinal(allocation));
        }
    }

    [TestFixture]
    public class ComputeIterationTests : HrTests
    {
        [Test]
        public void WhenPreferencesOfStudentEmpty()
        {
            //A  ни к кому не обратится - будет не распределен

            var students = new List<IToOneAllocated>
            {
                new ComplexToOneAllocated("a")
            };
            var lecturers = new List<IToManyAllocated>
            {
                new ComplexToManyAllocated("A")
            };

            var allocation = new OneToManyAllocation(lecturers, students);
            students[0].SetPreferences(new List<IAllocated>()); // a:
            lecturers[0].SetPreferences(new List<IAllocated>()); // A:
            lecturers[0].SetCapacity(2);

            _alg.computeIteration(allocation);

            var resultString = PrintUtilsV2Old.ToString(allocation.GetAllocationResult());

            Console.WriteLine(resultString);
            // a should be rejected
            //Should be A_ _a
            Assert.AreEqual("[A:], [:a], ", resultString);
        }

        [Test]
        public void WhenStundentIsNotAcceptable()
        {
            //Студент а обратиться но будет отвергнут 
            var students = new List<IToOneAllocated>
            {
                new ComplexToOneAllocated("a")
            };
            var lecturers = new List<IToManyAllocated>
            {
                new ComplexToManyAllocated("A")
            };

            var allocation = new OneToManyAllocation(lecturers, students);
            students[0].SetPreferences(new List<IAllocated> { lecturers[0] }); // a: A
            lecturers[0].SetPreferences(new List<IAllocated>()); // A:
            lecturers[0].SetCapacity(2);

            _alg.computeIteration(allocation);

            var resultString = PrintUtilsV2Old.ToString(allocation.GetAllocationResult());

            Console.WriteLine(resultString);

            //Should be A_ _a
            Assert.AreEqual("[A:], [:a], ", resultString);
        }

        [Test]
        public void WhenQuotaNotFull()
        {
            //Студент а обратиться и получит согласие
            var students = new List<IToOneAllocated>
            {
                new ComplexToOneAllocated("a")
            };
            var lecturers = new List<IToManyAllocated>
            {
                new ComplexToManyAllocated("A")
            };

            var allocation = new OneToManyAllocation(lecturers, students);
            students[0].SetPreferences(new List<IAllocated> { lecturers[0] }); // a: A
            lecturers[0].SetPreferences(new List<IAllocated> { students[0] }); // A: a
            lecturers[0].SetCapacity(2);

            _alg.computeIteration(allocation);

            var resultString = PrintUtilsV2Old.ToString(allocation.GetAllocationResult());

            Console.WriteLine(resultString);
            Assert.AreEqual("[A:a], ", resultString);
            //Should be Aa
        }

        [Test]
        public void WhenQuotaIsFull()
        {
            //Студент а обратиться и получит согласие
            var students = new List<IToOneAllocated>
            {
                new ComplexToOneAllocated("a"), new ComplexToOneAllocated("b")
            };
            var lecturers = new List<IToManyAllocated>
            {
                new ComplexToManyAllocated("A")
            };

            var allocation = new OneToManyAllocation(lecturers, students);
            students[0].SetPreferences(new List<IAllocated> { lecturers[0] }); // a: A
            students[1].SetPreferences(new List<IAllocated> { lecturers[0] }); // b: A
            lecturers[0].SetPreferences(new List<IAllocated> { students[0], students[1] }); // A: a b
            lecturers[0].SetCapacity(2);

            _alg.computeIteration(allocation);

            var resultString = PrintUtilsV2Old.ToString(allocation.GetAllocationResult());

            Console.WriteLine(resultString);
            //Should be Aab
            Assert.AreEqual("[A:ab], ", resultString);
        }

        [Test]
        public void whenQuotaIsFull_AndOverQuotaIsWorse()
        {
            //Студент с обратиться и будет отвергнут так как не влезет в квоту
            var students = new List<IToOneAllocated>
            {
                new ComplexToOneAllocated("a"), new ComplexToOneAllocated("b"), new ComplexToOneAllocated("c")
            };
            var lecturers = new List<IToManyAllocated>
            {
                new ComplexToManyAllocated("A")
            };

            var allocation = new OneToManyAllocation(lecturers, students);
            students[0].SetPreferences(new List<IAllocated> { lecturers[0] }); // a: A
            students[1].SetPreferences(new List<IAllocated> { lecturers[0] }); // b: A
            students[2].SetPreferences(new List<IAllocated> { lecturers[0] }); // c: A
            lecturers[0].SetPreferences(new List<IAllocated> { students[0], students[1], students[2] }); // A: a b c
            lecturers[0].SetCapacity(2);

            _alg.computeIteration(allocation);
            var resultString = PrintUtilsV2Old.ToString(allocation.GetAllocationResult());
            Console.WriteLine(resultString);
            //Should be Aab _c
            //c should be rejected
            Assert.AreEqual("[A:ab], [:c], ", resultString);
        }

        [Test]
        public void whenQuotaIsFull_AndOneOverQuotaAndIsBetter()
        {
            //Студент с обратиться и будет отвергнут так как не влезет в квоту
            var students = new List<IToOneAllocated>
            {
                new ComplexToOneAllocated("a"), new ComplexToOneAllocated("b"), new ComplexToOneAllocated("c")
            };
            var lecturers = new List<IToManyAllocated>
            {
                new ComplexToManyAllocated("A")
            };

            var allocation = new OneToManyAllocation(lecturers, students);
            students[0].SetPreferences(new List<IAllocated> { lecturers[0] }); // a: A
            students[1].SetPreferences(new List<IAllocated> { lecturers[0] }); // b: A
            students[2].SetPreferences(new List<IAllocated> { lecturers[0] }); // c: A
            lecturers[0].SetPreferences(new List<IAllocated> { students[2], students[0], students[1] }); // A: с a b
            lecturers[0].SetCapacity(2);

            _alg.computeIteration(allocation);
            var resultString = PrintUtilsV2Old.ToString(allocation.GetAllocationResult());
            Console.WriteLine(resultString);

            Console.WriteLine(resultString);
            //Should be Acb _b
            Assert.AreEqual("[A:ac], [:b], ", resultString);
        }
    }
}