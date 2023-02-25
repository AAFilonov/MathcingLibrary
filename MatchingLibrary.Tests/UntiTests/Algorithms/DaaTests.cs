using MatchingLibrary.Algorithms.impl;
using MatchingLibrary.Allocation.impl;
using MatchingLibrary.Tests.Utils;
using MatchingLibrary.Utils;
using NUnit.Framework;

namespace MatchingLibrary.Tests.UntiTests.Algorithms;

[TestFixture]
public class DaaTests
{
    private DAAAlgorithm<SimpleAllocated, SimpleAllocated> alg;

    [SetUp]
    public void Setup()
    {
        alg = new DAAAlgorithm<SimpleAllocated, SimpleAllocated>();
    }

    [TestFixture]
    public class IsFinalTests : DaaTests
    {
        [Test]
        public void testIsFinal_WhenListsNotEmptyAndNoPairs()
        {
            List<SimpleAllocated> men = new List<SimpleAllocated>()
                { new("a"), new("b"), new("c") };
            List<SimpleAllocated> women = new List<SimpleAllocated>()
                { new("A"), new("B"), new("C") };


            var allocation = new SmpAllocation<SimpleAllocated, SimpleAllocated>(men, women);
            allocation.setTPreferences(men[0], new List<SimpleAllocated>() { women[0], women[1], women[2] });
            allocation.setTPreferences(men[1], new List<SimpleAllocated>() { women[1], women[2], women[0] });
            allocation.setTPreferences(men[2], new List<SimpleAllocated>() { women[2], women[0], women[1] });

            var expectedIsFinal = false;
            var actualIsFinal = alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenListsEmptyAndNoPairs()
        {
            List<SimpleAllocated> men = new List<SimpleAllocated>()
                { new("a"), new("b"), new("c") };
            List<SimpleAllocated> women = new List<SimpleAllocated>()
                { new("A"), new("B"), new("C") };


            var allocation = new SmpAllocation<SimpleAllocated, SimpleAllocated>(men, women);
            allocation.setTPreferences(men[0], new List<SimpleAllocated>() { });
            allocation.setTPreferences(men[1], new List<SimpleAllocated>() { });
            allocation.setTPreferences(men[2], new List<SimpleAllocated>() { });

            var expectedIsFinal = true;
            var actualIsFinal = alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenNoListsProvided()
        {
            List<SimpleAllocated> men = new List<SimpleAllocated>()
                { new("a"), new("b"), new("c") };
            List<SimpleAllocated> women = new List<SimpleAllocated>()
                { new("A"), new("B"), new("C") };

            var allocation = new SmpAllocation<SimpleAllocated, SimpleAllocated>(men, women);

            var expectedIsFinal = true;
            var actualIsFinal = alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenListsAreEmptyAndThereAreSomePairs()
        {
            List<SimpleAllocated> men = new List<SimpleAllocated>()
                { new("a"), new("b"), new("c") };
            List<SimpleAllocated> women = new List<SimpleAllocated>()
                { new("A"), new("B"), new("C") };

            var allocation = new SmpAllocation<SimpleAllocated, SimpleAllocated>(men, women);

            allocation.setTPreferences(men[0], new List<SimpleAllocated>() { women[0], women[1], women[2] });
            allocation.setTPreferences(men[1], new List<SimpleAllocated>() { women[1], women[2], women[0] });
            allocation.setTPreferences(men[2], new List<SimpleAllocated>() { women[2], women[0], women[1] });

            allocation.assign(men[0], women[0]);
            allocation.assign(men[1], women[1]);
            var expectedIsFinal = false;
            var actualIsFinal = alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenListsAreNotEmptyAndThereAreSomePairs()
        {
            List<SimpleAllocated> men = new List<SimpleAllocated>()
                { new("a"), new("b"), new("c") };
            List<SimpleAllocated> women = new List<SimpleAllocated>()
                { new("A"), new("B"), new("C") };

            var allocation = new SmpAllocation<SimpleAllocated, SimpleAllocated>(men, women);
            allocation.assign(men[0], women[0]);
            allocation.assign(men[1], women[1]);
            //Для последнего достижимых нет
            var expectedIsFinal = true;
            var actualIsFinal = alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }


        [Test]
        public void testIsFinal_WhenListsNotEmptyAndAllArePaired()
        {
            List<SimpleAllocated> men = new List<SimpleAllocated>()
                { new("a"), new("b"), new("c") };
            List<SimpleAllocated> women = new List<SimpleAllocated>()
                { new("A"), new("B"), new("C") };

            var allocation = new SmpAllocation<SimpleAllocated, SimpleAllocated>(men, women);
            allocation.assign(men[0], women[0]);
            allocation.assign(men[1], women[1]);
            allocation.assign(men[2], women[2]);
            var expectedIsFinal = true;
            var actualIsFinal = alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }
    }

    [TestFixture]
    public class ComputeIterationTests : DaaTests
    {
        [Test]
        public void WhenEveryoneGetFirstDesired()
        {
           
            List<SimpleAllocated> men = new List<SimpleAllocated>()
                { new("a"), new("b"), new("c") };
            List<SimpleAllocated> women = new List<SimpleAllocated>()
                { new("A"), new("B"), new("C") };


            var allocation = new SmpAllocation<SimpleAllocated, SimpleAllocated>(men,women);
            allocation.setTPreferences(men[0], new List<SimpleAllocated>() { women[0], women[1], women[2] }); //A B C
            allocation.setTPreferences(men[1], new List<SimpleAllocated>() { women[1], women[2], women[0] }); //B C A
            allocation.setTPreferences(men[2], new List<SimpleAllocated>() { women[2], women[0], women[1] }); //C A B
        
            allocation.setUPreferences( women[0], new List<SimpleAllocated>() { men[0] });  //a
            allocation.setUPreferences( women[1], new List<SimpleAllocated>() { men[1] });  //b
            allocation.setUPreferences( women[2], new List<SimpleAllocated>() { men[2] });  //c

            alg.computeIteration(allocation);

            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            
            Console.WriteLine(resultString);
            // all assigned to first person in list
            //should be AA BB CC
            Assert.AreEqual("pair: [a:A],pair: [b:B],pair: [c:C],", resultString);
        
        }

        [Test]
        public void WhenSomeStaySingle()
        {
           
            List<SimpleAllocated> men = new List<SimpleAllocated>()
                { new("a"), new("b"), new("c") };
            List<SimpleAllocated> women = new List<SimpleAllocated>()
                { new("A"), new("B"), new("C") };


            var allocation = new SmpAllocation<SimpleAllocated, SimpleAllocated>(men,women);
            allocation.setTPreferences(men[0], new List<SimpleAllocated>() { women[0], women[1], women[2] }); //A B C
            allocation.setTPreferences(men[1], new List<SimpleAllocated>() { women[0], women[2] });           //A C 
            allocation.setTPreferences(men[2], new List<SimpleAllocated>() { women[2], women[0], women[1] }); //C A B
        
            allocation.setUPreferences( women[0], new List<SimpleAllocated>() { men[0] });             //A
            allocation.setUPreferences( women[1], new List<SimpleAllocated>() { men[0] });             //A
            allocation.setUPreferences( women[2], new List<SimpleAllocated>() { men[2] , men[1] });    //C B
            alg.computeIteration(allocation);

            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            
            Console.WriteLine(resultString);
            // all assigned to first person in list
            //should be AA CC B_ _B 
            Assert.AreEqual("pair: [a:A],pair: [c:C],", resultString);
        
        }
    }
}