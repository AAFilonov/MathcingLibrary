using MatchingLibrary.Tests.Utils;
using MatchingLibrary.v2.Algorithms;
using MatchingLibrary.v2.Allocated;
using MatchingLibrary.v2.Allocation;
using NUnit.Framework;

namespace MatchingLibrary.Tests.v2;

[TestFixture]
public class DaaTests
{
    private SmpDaaMaleAlgorithm alg;

    [SetUp]
    public void Setup()
    {
        alg = new SmpDaaMaleAlgorithm();
    }

    [TestFixture]
    public class IsFinalTests : DaaTests
    {
        [Test]
        public void testIsFinal_WhenListsNotEmptyAndNoPairs()
        {
            List<IOneToOneAllocated> men = new List<IOneToOneAllocated>()
                {new ComplexOneToOneAllocated("a"), new ComplexOneToOneAllocated("b"), new ComplexOneToOneAllocated("c")};
            List<IOneToOneAllocated> women = new List<IOneToOneAllocated>()
                {new ComplexOneToOneAllocated("A"), new ComplexOneToOneAllocated("B"), new ComplexOneToOneAllocated("C")};


            var allocation = new SmpAllocation(men, women);
            men[0].SetPreferences(new List<IOneToOneAllocated>() {women[0], women[1], women[2]});
            men[1].SetPreferences(new List<IOneToOneAllocated>() {women[1], women[2], women[0]});
            men[2].SetPreferences(new List<IOneToOneAllocated>() {women[2], women[0], women[1]});

            var expectedIsFinal = false;
            var actualIsFinal = alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenListsEmptyAndNoPairs()
        {
            List<IOneToOneAllocated> men = new List<IOneToOneAllocated>()
                {new ComplexOneToOneAllocated("a"), new ComplexOneToOneAllocated("b"), new ComplexOneToOneAllocated("c")};
            List<IOneToOneAllocated> women = new List<IOneToOneAllocated>()
                {new ComplexOneToOneAllocated("A"), new ComplexOneToOneAllocated("B"), new ComplexOneToOneAllocated("C")};


            var allocation = new SmpAllocation(men, women);
            men[0].SetPreferences(new List<IOneToOneAllocated>() { });
            men[1].SetPreferences(new List<IOneToOneAllocated>() { });
            men[2].SetPreferences(new List<IOneToOneAllocated>() { });

            var expectedIsFinal = true;
            var actualIsFinal = alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenNoListsProvided()
        {
            List<IOneToOneAllocated> men = new List<IOneToOneAllocated>()
                {new ComplexOneToOneAllocated("a"), new ComplexOneToOneAllocated("b"), new ComplexOneToOneAllocated("c")};
            List<IOneToOneAllocated> women = new List<IOneToOneAllocated>()
                {new ComplexOneToOneAllocated("A"), new ComplexOneToOneAllocated("B"), new ComplexOneToOneAllocated("C")};


            var allocation = new SmpAllocation(men, women);
            var expectedIsFinal = true;
            var actualIsFinal = alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenListsAreEmptyAndThereAreSomePairs()
        {
            List<IOneToOneAllocated> men = new List<IOneToOneAllocated>()
                {new ComplexOneToOneAllocated("a"), new ComplexOneToOneAllocated("b"), new ComplexOneToOneAllocated("c")};
            List<IOneToOneAllocated> women = new List<IOneToOneAllocated>()
                {new ComplexOneToOneAllocated("A"), new ComplexOneToOneAllocated("B"), new ComplexOneToOneAllocated("C")};


            var allocation = new SmpAllocation(men, women);
            men[0].SetPreferences(new List<IOneToOneAllocated>() {women[0], women[1], women[2]});
            men[1].SetPreferences(new List<IOneToOneAllocated>() {women[1], women[2], women[0]});
            men[2].SetPreferences(new List<IOneToOneAllocated>() {women[2], women[0], women[1]});

            men[0].Assign(women[0]);
            men[1].Assign(women[1]);

            var expectedIsFinal = false;
            var actualIsFinal = alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenListsAreNotEmptyAndThereAreSomePairs()
        {
            List<IOneToOneAllocated> men = new List<IOneToOneAllocated>()
                {new ComplexOneToOneAllocated("a"), new ComplexOneToOneAllocated("b"), new ComplexOneToOneAllocated("c")};
            List<IOneToOneAllocated> women = new List<IOneToOneAllocated>()
                {new ComplexOneToOneAllocated("A"), new ComplexOneToOneAllocated("B"), new ComplexOneToOneAllocated("C")};


            var allocation = new SmpAllocation(men, women);
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
            List<IOneToOneAllocated> men = new List<IOneToOneAllocated>()
                {new ComplexOneToOneAllocated("a"), new ComplexOneToOneAllocated("b"), new ComplexOneToOneAllocated("c")};
            List<IOneToOneAllocated> women = new List<IOneToOneAllocated>()
                {new ComplexOneToOneAllocated("A"), new ComplexOneToOneAllocated("B"), new ComplexOneToOneAllocated("C")};


            var allocation = new SmpAllocation(men, women);
            men[0].SetPreferences(new List<IOneToOneAllocated>() {women[0], women[1], women[2]});
            men[1].SetPreferences(new List<IOneToOneAllocated>() {women[1], women[2], women[0]});
            men[2].SetPreferences(new List<IOneToOneAllocated>() {women[2], women[0], women[1]});

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
            List<IOneToOneAllocated> men = new List<IOneToOneAllocated>()
                {new ComplexOneToOneAllocated("a"), new ComplexOneToOneAllocated("b"), new ComplexOneToOneAllocated("c")};
            List<IOneToOneAllocated> women = new List<IOneToOneAllocated>()
                {new ComplexOneToOneAllocated("A"), new ComplexOneToOneAllocated("B"), new ComplexOneToOneAllocated("C")};


            var allocation = new SmpAllocation(men, women);
            men[0].SetPreferences(new List<IOneToOneAllocated>() {women[0], women[1], women[2]}); //A B C
            men[1].SetPreferences(new List<IOneToOneAllocated>() {women[1], women[2], women[0]}); //B C A
            men[2].SetPreferences(new List<IOneToOneAllocated>() {women[2], women[0], women[1]}); //C A B

            women[0].SetPreferences(new List<IOneToOneAllocated>() {men[0]}); //a
            women[1].SetPreferences(new List<IOneToOneAllocated>() {men[1]}); //b
            women[2].SetPreferences(new List<IOneToOneAllocated>() {men[2]}); //c


            alg.computeIteration(allocation);

            var result = allocation.CalcFinalAllocation();
            var resultString = PrintUtilsV2.toString2(result);

            Console.WriteLine(resultString);
            // all assigned to first person in list
            //should be AA BB CC
            Assert.AreEqual("pair: [a:A],pair: [b:B],pair: [c:C],", resultString);
        }

        [Test]
        public void WhenSomeStaySingle()
        {
            List<IOneToOneAllocated> men = new List<IOneToOneAllocated>()
                {new ComplexOneToOneAllocated("a"), new ComplexOneToOneAllocated("b"), new ComplexOneToOneAllocated("c")};
            List<IOneToOneAllocated> women = new List<IOneToOneAllocated>()
                {new ComplexOneToOneAllocated("A"), new ComplexOneToOneAllocated("B"), new ComplexOneToOneAllocated("C")};


            var allocation = new SmpAllocation(men, women);
            men[0].SetPreferences(new List<IOneToOneAllocated>() {women[0], women[1], women[2]}); //A B C
            men[1].SetPreferences(new List<IOneToOneAllocated>() {women[0], women[2]}); //A C
            men[2].SetPreferences(new List<IOneToOneAllocated>() {women[2], women[0], women[1]}); //C A B

            women[0].SetPreferences(new List<IOneToOneAllocated>() {men[0]}); //a
            women[1].SetPreferences(new List<IOneToOneAllocated>() {men[0]}); //a
            women[2].SetPreferences(new List<IOneToOneAllocated>() {men[2], men[1]}); //c b

            alg.computeIteration(allocation);

            var result = allocation.CalcFinalAllocation();
            var resultString = PrintUtilsV2.toString2(result);

            Console.WriteLine(resultString);
            // all assigned to first person in list
            //should be AA CC B_ _B 
            Assert.AreEqual("pair: [a:A],pair: [b:],pair: [c:C],pair: [:B],", resultString);
        }
    }
}