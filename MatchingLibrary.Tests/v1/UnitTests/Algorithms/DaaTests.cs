using MatchingLibrary.Tests.Utils;
using MatchingLibrary.v1.Algorithms.impl.Smp;
using MatchingLibrary.v1.Allocated.Impl;
using MatchingLibrary.v1.Allocation.impl.Smp;
using NUnit.Framework;

namespace MatchingLibrary.Tests.v1.UnitTests.Algorithms;

[TestFixture]
public class DaaTests
{
    [SetUp]
    public void Setup()
    {
        _alg = new DAAAlgorithm<NamedAllocated, NamedAllocated?>();
    }

    private DAAAlgorithm<NamedAllocated, NamedAllocated?> _alg;

    [TestFixture]
    public class IsFinalTests : DaaTests
    {
        [Test]
        public void testIsFinal_WhenListsNotEmptyAndNoPairs()
        {
            var men = new List<NamedAllocated>
                { new("a"), new("b"), new("c") };
            var women = new List<NamedAllocated>
                { new("A"), new("B"), new("C") };


            var allocation = new SmpAllocationGeneric<NamedAllocated, NamedAllocated>(men, women);
            allocation.setTPreferences(men[0], new List<NamedAllocated> { women[0], women[1], women[2] });
            allocation.setTPreferences(men[1], new List<NamedAllocated> { women[1], women[2], women[0] });
            allocation.setTPreferences(men[2], new List<NamedAllocated> { women[2], women[0], women[1] });

            var expectedIsFinal = false;
            var actualIsFinal = _alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenListsEmptyAndNoPairs()
        {
            var men = new List<NamedAllocated>
                { new("a"), new("b"), new("c") };
            var women = new List<NamedAllocated>
                { new("A"), new("B"), new("C") };


            var allocation = new SmpAllocationGeneric<NamedAllocated, NamedAllocated?>(men, women);
            allocation.setTPreferences(men[0], new List<NamedAllocated?>());
            allocation.setTPreferences(men[1], new List<NamedAllocated?>());
            allocation.setTPreferences(men[2], new List<NamedAllocated?>());

            var expectedIsFinal = true;
            var actualIsFinal = _alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenNoListsProvided()
        {
            var men = new List<NamedAllocated>
                { new("a"), new("b"), new("c") };
            var women = new List<NamedAllocated>
                { new("A"), new("B"), new("C") };

            var allocation = new SmpAllocationGeneric<NamedAllocated, NamedAllocated?>(men, women);

            var expectedIsFinal = true;
            var actualIsFinal = _alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenListsAreEmptyAndThereAreSomePairs()
        {
            var men = new List<NamedAllocated>
                { new("a"), new("b"), new("c") };
            var women = new List<NamedAllocated?>
                { new("A"), new("B"), new("C") };

            var allocation = new SmpAllocationGeneric<NamedAllocated, NamedAllocated?>(men, women);

            allocation.setTPreferences(men[0], new List<NamedAllocated?> { women[0], women[1], women[2] });
            allocation.setTPreferences(men[1], new List<NamedAllocated?> { women[1], women[2], women[0] });
            allocation.setTPreferences(men[2], new List<NamedAllocated?> { women[2], women[0], women[1] });

            allocation.assign(men[0], women[0]);
            allocation.assign(men[1], women[1]);
            var expectedIsFinal = false;
            var actualIsFinal = _alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenListsAreNotEmptyAndThereAreSomePairs()
        {
            var men = new List<NamedAllocated>
                { new("a"), new("b"), new("c") };
            var women = new List<NamedAllocated?>
                { new("A"), new("B"), new("C") };

            var allocation = new SmpAllocationGeneric<NamedAllocated, NamedAllocated?>(men, women);
            allocation.assign(men[0], women[0]);
            allocation.assign(men[1], women[1]);
            //Для последнего достижимых нет
            var expectedIsFinal = true;
            var actualIsFinal = _alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }


        [Test]
        public void testIsFinal_WhenListsNotEmptyAndAllArePaired()
        {
            var men = new List<NamedAllocated>
                { new("a"), new("b"), new("c") };
            var women = new List<NamedAllocated?>
                { new("A"), new("B"), new("C") };

            var allocation = new SmpAllocationGeneric<NamedAllocated, NamedAllocated?>(men, women);
            allocation.assign(men[0], women[0]);
            allocation.assign(men[1], women[1]);
            allocation.assign(men[2], women[2]);
            var expectedIsFinal = true;
            var actualIsFinal = _alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }
    }

    [TestFixture]
    public class ComputeIterationTests : DaaTests
    {
        [Test]
        public void WhenEveryoneGetFirstDesired()
        {
            var men = new List<NamedAllocated>
                { new("a"), new("b"), new("c") };
            var women = new List<NamedAllocated?>
                { new("A"), new("B"), new("C") };


            var allocation = new SmpAllocationGeneric<NamedAllocated, NamedAllocated?>(men, women);
            allocation.setTPreferences(men[0], new List<NamedAllocated?> { women[0], women[1], women[2] }); //A B C
            allocation.setTPreferences(men[1], new List<NamedAllocated?> { women[1], women[2], women[0] }); //B C A
            allocation.setTPreferences(men[2], new List<NamedAllocated?> { women[2], women[0], women[1] }); //C A B

            allocation.setUPreferences(women[0], new List<NamedAllocated> { men[0] }); //a
            allocation.setUPreferences(women[1], new List<NamedAllocated> { men[1] }); //b
            allocation.setUPreferences(women[2], new List<NamedAllocated> { men[2] }); //c

            _alg.computeIteration(allocation);

            var resultString = PrintUtils.ToString(allocation.calcFinalAllocation());

            Console.WriteLine(resultString);
            // all assigned to first person in list
            //should be AA BB CC
            Assert.AreEqual("pair: [a:A],pair: [b:B],pair: [c:C],", resultString);
        }

        [Test]
        public void WhenSomeStaySingle()
        {
            var men = new List<NamedAllocated>
                { new("a"), new("b"), new("c") };
            var women = new List<NamedAllocated?>
                { new("A"), new("B"), new("C") };


            var allocation = new SmpAllocationGeneric<NamedAllocated, NamedAllocated?>(men, women);
            allocation.setTPreferences(men[0], new List<NamedAllocated?> { women[0], women[1], women[2] }); //A B C
            allocation.setTPreferences(men[1], new List<NamedAllocated?> { women[0], women[2] }); //A C 
            allocation.setTPreferences(men[2], new List<NamedAllocated?> { women[2], women[0], women[1] }); //C A B

            allocation.setUPreferences(women[0], new List<NamedAllocated> { men[0] }); //A
            allocation.setUPreferences(women[1], new List<NamedAllocated> { men[0] }); //A
            allocation.setUPreferences(women[2], new List<NamedAllocated> { men[2], men[1] }); //C B
            _alg.computeIteration(allocation);

            var resultString = PrintUtils.ToString(allocation.calcFinalAllocation());

            Console.WriteLine(resultString);
            // all assigned to first person in list
            //should be AA CC B_ _B 
            Assert.AreEqual("pair: [a:A],pair: [c:C],", resultString);
        }
    }
}