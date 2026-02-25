using MatchingLibrary.Allocated;
using MatchingLibrary.Allocated.impl;
using MatchingLibrary.Allocation;
using MatchingLibrary.Allocators;
using MatchingLibrary.Tests.Utils;
using MatchingLibrary.Utils;
using NUnit.Framework;

namespace MatchingLibrary.Tests.unitTests;

[TestFixture]
public class HrResidentTests
{
    private HrResidentAllocator _alg = new HrResidentAllocator();

    [TestFixture]
    public class IsFinalResidentTests : HrResidentTests
    {
        /// <summary>
        /// Проверяется, что распределение считается финальным,
        /// если предпочтения отсутствуют и пар нет.
        /// Вход: 3 студента, 3 преподавателя, без предпочтений.
        /// Ожидается: isFinal == true.
        /// </summary>
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

        /// <summary>
        /// Проверяется, что алгоритм не финален,
        /// если существуют предпочтения и возможные предложения.
        /// Вход: студент a предпочитает A,B; A принимает a.
        /// Ожидается: isFinal == false.
        /// </summary>
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
            lecturers[0].SetCapacity(2);
            lecturers[1].SetCapacity(2);
            lecturers[2].SetCapacity(2);

            var allocation = new OneToManyAllocation(lecturers, students);
            students[0].SetPreferences(new List<IAllocated> { lecturers[0], lecturers[1] }); //A B
            lecturers[0].SetPreferences(new List<IAllocated> { students[0] }); //a

            Assert.AreEqual(false, _alg.isFinal(allocation));
        }

        /// <summary>
        /// Проверяется, что распределение финально,
        /// если все участники уже распределены.
        /// Вход: каждому студенту назначен свой преподаватель.
        /// Ожидается: isFinal == true.
        /// </summary>
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
    public class ComputeIterationResidentTests : HrResidentTests
    {
        /// <summary>
        /// Проверяется, что студент без предпочтений остаётся нераспределён.
        /// Вход: a без предпочтений; A без предпочтений.
        /// Ожидается: A пусто, a не распределён.
        /// </summary>
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

        /// <summary>
        /// Проверяется отказ, если студент неприемлем для преподавателя.
        /// Вход: a -> A; A не содержит a в предпочтениях.
        /// Ожидается: A пусто, a не распределён.
        /// </summary>
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

        /// <summary>
        /// Проверяется принятие при неполной квоте.
        /// Вход: a -> A; A предпочитает a; capacity=2.
        /// Ожидается: A:a.
        /// </summary>
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

        /// <summary>
        /// Проверяется заполнение квоты без отказов.
        /// Вход: a,b -> A; A: a>b; capacity=2.
        /// Ожидается: A:ab.
        /// </summary>
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

        /// <summary>
        /// Проверяется отказ худшего при переполнении квоты.
        /// Вход: a,b,c -> A; A: a>b>c; capacity=2.
        /// Ожидается: A:ab, c отклонён.
        /// </summary>
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

        /// <summary>
        /// Проверяется замещение худшего более предпочтительным.
        /// Вход: a,b,c -> A; A: c>a>b; capacity=2.
        /// Ожидается: A:ac, b отклонён.
        /// </summary>
        [Test]
        public void whenQuotaIsFull_AndOneOverQuotaAndIsBetter()
        {
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
    public class AppResidentTest : HrResidentTests
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

    /// <summary>
    /// Проверяется, что RGS возвращает resident-optimal стабильное распределение.
    ///
    /// Вход:
    /// R1: H1 > H2
    /// R2: H1 > H2
    /// R3: H2 > H1
    ///
    /// H1 (2): R3 > R1 > R2
    /// H2 (1): R1 > R2 > R3
    ///
    /// Ожидаемый результат (resident-optimal):
    /// H1: R1 R2
    /// H2: R3
    /// </summary>
    [Test]
    public void RGS_Should_Return_ResidentOptimal_Matching()
    {
        var residents = new List<IToOneAllocated>
        {
            new ComplexToOneAllocated("R1"),
            new ComplexToOneAllocated("R2"),
            new ComplexToOneAllocated("R3")
        };

        var hospitals = new List<IToManyAllocated>
        {
            new ComplexToManyAllocated("H1"),
            new ComplexToManyAllocated("H2")
        };

        var allocation = new OneToManyAllocation(hospitals, residents);

        // Preferences of residents
        residents[0].SetPreferences(new List<IAllocated> { hospitals[0], hospitals[1] }); // R1: H1 > H2
        residents[1].SetPreferences(new List<IAllocated> { hospitals[0], hospitals[1] }); // R2: H1 > H2
        residents[2].SetPreferences(new List<IAllocated> { hospitals[1], hospitals[0] }); // R3: H2 > H1

        // Preferences of hospitals
        hospitals[0].SetPreferences(new List<IAllocated>
            { residents[2], residents[0], residents[1] }); // H1: R3 > R1 > R2
        hospitals[1].SetPreferences(new List<IAllocated>
            { residents[0], residents[1], residents[2] }); // H2: R1 > R2 > R3

        hospitals[0].SetCapacity(2);
        hospitals[1].SetCapacity(1);

        _alg.computeIteration(allocation); // RGS implementation

        var resultString = PrintUtilsV2.ToString(allocation.GetAllocationResult());
        Console.WriteLine(resultString);

        // Expected:
        // H1: R1 R2
        // H2: R3
        Assert.AreEqual("[H1:R1R2], [H2:R3], ", resultString);
    }
}