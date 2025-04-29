using MatchingLibrary.Allocated;
using MatchingLibrary.Allocated.impl;
using MatchingLibrary.Allocation;
using MatchingLibrary.Allocators;
using MatchingLibrary.Tests.Utils;
using MatchingLibrary.Utils;
using NUnit.Framework;

namespace MatchingLibrary.Tests.unitTests;

[TestFixture]
public class HrTests
{
 
    private HrResidentAllocator _alg = new HrResidentAllocator();

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

            var resultString = PrintUtilsV2.ToString(allocation.GetAllocationResult());

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

            var resultString = PrintUtilsV2.ToString(allocation.GetAllocationResult());

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

            var resultString = PrintUtilsV2.ToString(allocation.GetAllocationResult());

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

            var resultString = PrintUtilsV2.ToString(allocation.GetAllocationResult());

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
            var resultString = PrintUtilsV2.ToString(allocation.GetAllocationResult());
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
            var resultString = PrintUtilsV2.ToString(allocation.GetAllocationResult());
            Console.WriteLine(resultString);

            Console.WriteLine(resultString);
            //Should be Acb _b
            Assert.AreEqual("[A:ac], [:b], ", resultString);
        }
    }

    [TestFixture]
    public class AppTest : HrTests
    {
        [Test]
        public void IMMB_Example()
        {
            var subordinates = new List<IToOneAllocated>
            {
                new ComplexToOneAllocated("a"),
                new ComplexToOneAllocated("b"),
                new ComplexToOneAllocated("c"),
                new ComplexToOneAllocated("d"),
                new ComplexToOneAllocated("e"),
                new ComplexToOneAllocated("f"),
                new ComplexToOneAllocated("g")
            };
            var leaders = new List<IToManyAllocated>
            {
                new ComplexToManyAllocated("A", 3),
                new ComplexToManyAllocated("B", 3),
                new ComplexToManyAllocated("C", 2)
            };

            var allocation = new OneToManyAllocation(leaders, subordinates);
            subordinates[0].SetPreferences(new List<IAllocated> { leaders[0], leaders[1], leaders[2] }); // a: A B C
            subordinates[1].SetPreferences(new List<IAllocated> { leaders[2], leaders[0] }); // b: C A
            subordinates[2].SetPreferences(new List<IAllocated> { leaders[1], leaders[2], leaders[0] }); // c: B C A
            subordinates[3].SetPreferences(new List<IAllocated> { leaders[2], leaders[0], leaders[1] }); // d: C A B
            subordinates[4].SetPreferences(new List<IAllocated> { leaders[0], leaders[2], leaders[1] }); // e: A C B
            subordinates[5].SetPreferences(new List<IAllocated> { leaders[2], leaders[0] }); // f: C A
            subordinates[6].SetPreferences(new List<IAllocated> { leaders[0], leaders[1], leaders[2] }); // g: A B C
            // a b c d e f g
            // 0 1 2 3 4 5 6        
            leaders[0].SetPreferences(new List<IAllocated>
            {
                // A: b f a g c e d
                subordinates[1],
                subordinates[5],
                subordinates[0],
                subordinates[6],
                subordinates[2],
                subordinates[4],
                subordinates[3]
            });
            leaders[1].SetPreferences(new List<IAllocated>
            {
                // B: a g e c d
                subordinates[0],
                subordinates[6],
                subordinates[4],
                subordinates[2],
                subordinates[3]
            });
            leaders[2].SetPreferences(new List<IAllocated>
            {
                // C: a b c e d f g
                subordinates[0],
                subordinates[1],
                subordinates[2],
                subordinates[4],
                subordinates[3],
                subordinates[5],
                subordinates[6]
            });

            string resultString;
            var alg = new HrResidentAllocator();
            // iteration 1
            alg.computeIteration(allocation);
            resultString = PrintUtilsV2.ToString(allocation.GetAllocationResult());
            Assert.AreEqual("[A:aeg], [B:c], [C:bd], [:f], ", resultString);
            // iteration 2
            alg.computeIteration(allocation);
            resultString = PrintUtilsV2.ToString(allocation.GetAllocationResult());
            Assert.AreEqual("[A:agf], [B:c], [C:bd], [:e], ", resultString);
            // iteration 3
            alg.computeIteration(allocation);
            resultString = PrintUtilsV2.ToString(allocation.GetAllocationResult());
            Assert.AreEqual("[A:agf], [B:c], [C:be], [:d], ", resultString);
            // iteration 4
            alg.computeIteration(allocation);
            resultString = PrintUtilsV2.ToString(allocation.GetAllocationResult());
            Assert.AreEqual("[A:agf], [B:c], [C:be], [:d], ", resultString);
            // iteration 5
            alg.computeIteration(allocation);
            resultString = PrintUtilsV2.ToString(allocation.GetAllocationResult());
            Assert.AreEqual("[A:agf], [B:cd], [C:be], ", resultString);
        }
    }
}