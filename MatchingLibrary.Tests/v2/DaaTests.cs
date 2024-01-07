using MatchingLibrary.Tests.Utils;
using MatchingLibrary.v2.Algorithms;
using MatchingLibrary.v2.Allocated;
using MatchingLibrary.v2.Allocated.impl;
using MatchingLibrary.v2.Allocation;
using NUnit.Framework;

namespace MatchingLibrary.Tests.v2;

[TestFixture]
public class DaaTests
{
    private SmpDaaMaleAllocator alg;

    [SetUp]
    public void Setup()
    {
        alg = new SmpDaaMaleAllocator();
    }

    [TestFixture]
    public class IsFinalTests : DaaTests
    {
        [Test]
        public void testIsFinal_WhenListsNotEmptyAndNoPairs()
        {
            List<IToOneAllocated> men = new List<IToOneAllocated>()
                {new ComplexToOneAllocated("a"), new ComplexToOneAllocated("b"), new ComplexToOneAllocated("c")};
            List<IToOneAllocated> women = new List<IToOneAllocated>()
                {new ComplexToOneAllocated("A"), new ComplexToOneAllocated("B"), new ComplexToOneAllocated("C")};


            var allocation = new OneToOneAllocation(men, women);
            men[0].SetPreferences(new List<IAllocated>() {women[0], women[1], women[2]});
            men[1].SetPreferences(new List<IAllocated>() {women[1], women[2], women[0]});
            men[2].SetPreferences(new List<IAllocated>() {women[2], women[0], women[1]});

            var expectedIsFinal = false;
            var actualIsFinal = alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenListsEmptyAndNoPairs()
        {
            List<IToOneAllocated> men = new List<IToOneAllocated>()
                {new ComplexToOneAllocated("a"), new ComplexToOneAllocated("b"), new ComplexToOneAllocated("c")};
            List<IToOneAllocated> women = new List<IToOneAllocated>()
                {new ComplexToOneAllocated("A"), new ComplexToOneAllocated("B"), new ComplexToOneAllocated("C")};


            var allocation = new OneToOneAllocation(men, women);
            men[0].SetPreferences(new List<IAllocated>() { });
            men[1].SetPreferences(new List<IAllocated>() { });
            men[2].SetPreferences(new List<IAllocated>() { });

            var expectedIsFinal = true;
            var actualIsFinal = alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenNoListsProvided()
        {
            List<IToOneAllocated> men = new List<IToOneAllocated>()
                {new ComplexToOneAllocated("a"), new ComplexToOneAllocated("b"), new ComplexToOneAllocated("c")};
            List<IToOneAllocated> women = new List<IToOneAllocated>()
                {new ComplexToOneAllocated("A"), new ComplexToOneAllocated("B"), new ComplexToOneAllocated("C")};


            var allocation = new OneToOneAllocation(men, women);
            var expectedIsFinal = true;
            var actualIsFinal = alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenListsAreEmptyAndThereAreSomePairs()
        {
            List<IToOneAllocated> men = new List<IToOneAllocated>()
                {new ComplexToOneAllocated("a"), new ComplexToOneAllocated("b"), new ComplexToOneAllocated("c")};
            List<IToOneAllocated> women = new List<IToOneAllocated>()
                {new ComplexToOneAllocated("A"), new ComplexToOneAllocated("B"), new ComplexToOneAllocated("C")};


            var allocation = new OneToOneAllocation(men, women);
            men[0].SetPreferences(new List<IAllocated>() {women[0], women[1], women[2]});
            men[1].SetPreferences(new List<IAllocated>() {women[1], women[2], women[0]});
            men[2].SetPreferences(new List<IAllocated>() {women[2], women[0], women[1]});

            men[0].Assign(women[0]);
            men[1].Assign(women[1]);

            var expectedIsFinal = false;
            var actualIsFinal = alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenListsAreNotEmptyAndThereAreSomePairs()
        {
            List<IToOneAllocated> men = new List<IToOneAllocated>()
                {new ComplexToOneAllocated("a"), new ComplexToOneAllocated("b"), new ComplexToOneAllocated("c")};
            List<IToOneAllocated> women = new List<IToOneAllocated>()
                {new ComplexToOneAllocated("A"), new ComplexToOneAllocated("B"), new ComplexToOneAllocated("C")};


            var allocation = new OneToOneAllocation(men, women);
            men[0].Assign(women[0]);
            men[1].Assign(women[1]);

            //Для последнего достижимых нет
            var expectedIsFinal = true;
            var actualIsFinal = alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }


        [Test]
        public void testIsFinal_WhenListsNotEmptyAndAllArePaired()
        {
            List<IToOneAllocated> men = new List<IToOneAllocated>()
                {new ComplexToOneAllocated("a"), new ComplexToOneAllocated("b"), new ComplexToOneAllocated("c")};
            List<IToOneAllocated> women = new List<IToOneAllocated>()
                {new ComplexToOneAllocated("A"), new ComplexToOneAllocated("B"), new ComplexToOneAllocated("C")};


            var allocation = new OneToOneAllocation(men, women);
            men[0].SetPreferences(new List<IAllocated>() {women[0], women[1], women[2]});
            men[1].SetPreferences(new List<IAllocated>() {women[1], women[2], women[0]});
            men[2].SetPreferences(new List<IAllocated>() {women[2], women[0], women[1]});

            men[0].Assign(women[0]);
            men[1].Assign(women[1]);
            men[2].Assign(women[2]);

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
            List<IToOneAllocated> men = new List<IToOneAllocated>()
                {new ComplexToOneAllocated("a"), new ComplexToOneAllocated("b"), new ComplexToOneAllocated("c")};
            List<IToOneAllocated> women = new List<IToOneAllocated>()
                {new ComplexToOneAllocated("A"), new ComplexToOneAllocated("B"), new ComplexToOneAllocated("C")};


            var allocation = new OneToOneAllocation(men, women);
            men[0].SetPreferences(new List<IAllocated>() {women[0], women[1], women[2]}); //A B C
            men[1].SetPreferences(new List<IAllocated>() {women[1], women[2], women[0]}); //B C A
            men[2].SetPreferences(new List<IAllocated>() {women[2], women[0], women[1]}); //C A B

            women[0].SetPreferences(new List<IAllocated>() {men[0]}); //a
            women[1].SetPreferences(new List<IAllocated>() {men[1]}); //b
            women[2].SetPreferences(new List<IAllocated>() {men[2]}); //c


            alg.computeIteration(allocation);

            var result = allocation.GetAllocationResult();
            var resultString = PrintUtilsV2.toString2(result);

            Console.WriteLine(resultString);
            // all assigned to first person in list
            //should be Aa Bb Cc
            Assert.AreEqual("[a:A],[b:B],[c:C],", resultString);
        }

        [Test]
        public void WhenSomeStaySingle()
        {
            List<IToOneAllocated> men = new List<IToOneAllocated>()
                {new ComplexToOneAllocated("a"), new ComplexToOneAllocated("b"), new ComplexToOneAllocated("c")};
            List<IToOneAllocated> women = new List<IToOneAllocated>()
                {new ComplexToOneAllocated("A"), new ComplexToOneAllocated("B"), new ComplexToOneAllocated("C")};


            var allocation = new OneToOneAllocation(men, women);
            men[0].SetPreferences(new List<IAllocated>() {women[0], women[1], women[2]}); //A B C
            men[1].SetPreferences(new List<IAllocated>() {women[0], women[2]}); //A C
            men[2].SetPreferences(new List<IAllocated>() {women[2], women[0], women[1]}); //C A B

            women[0].SetPreferences(new List<IAllocated>() {men[0]}); //a
            women[1].SetPreferences(new List<IAllocated>() {men[0]}); //a
            women[2].SetPreferences(new List<IAllocated>() {men[2], men[1]}); //c b

            alg.computeIteration(allocation);

            var result = allocation.GetAllocationResult();
            var resultString = PrintUtilsV2.toString2(result);

            Console.WriteLine(resultString);
            // all assigned to first person in list
            //should be AA CC B_ _B 
            Assert.AreEqual("[a:A],[b:],[c:C],[:B],", resultString);
        }
    }
}