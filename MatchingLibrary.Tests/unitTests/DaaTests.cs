using MatchingLibrary.Allocated;
using MatchingLibrary.Allocated.impl;
using MatchingLibrary.Allocation;
using MatchingLibrary.Allocators;
using MatchingLibrary.Tests.Utils;
using NUnit.Framework;

namespace MatchingLibrary.Tests.unitTests;

[TestFixture]
public class DaaTests
{
    private readonly SmpMaleAllocator _alg = new();

    [TestFixture]
    public class IsFinalTests : DaaTests
    {
        [Test]
        public void testIsFinal_WhenListsNotEmptyAndNoPairs()
        {
            var men = new List<IToOneAllocated>
            {
                new NamedToOneAllocated("a"),
                new NamedToOneAllocated("b"),
                new NamedToOneAllocated("c")
            };
            var women = new List<IToOneAllocated>
                { new NamedToOneAllocated("A"), new NamedToOneAllocated("B"), new NamedToOneAllocated("C") };


            var allocation = new OneToOneAllocation(men, women);
            men[0].SetPreferences(new List<IAllocated> { women[0], women[1], women[2] });
            men[1].SetPreferences(new List<IAllocated> { women[1], women[2], women[0] });
            men[2].SetPreferences(new List<IAllocated> { women[2], women[0], women[1] });

            var expectedIsFinal = false;
            var actualIsFinal = _alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenListsEmptyAndNoPairs()
        {
            var men = new List<IToOneAllocated>
                { new NamedToOneAllocated("a"), new NamedToOneAllocated("b"), new NamedToOneAllocated("c") };
            var women = new List<IToOneAllocated>
                { new NamedToOneAllocated("A"), new NamedToOneAllocated("B"), new NamedToOneAllocated("C") };


            var allocation = new OneToOneAllocation(men, women);
            men[0].SetPreferences(new List<IAllocated>());
            men[1].SetPreferences(new List<IAllocated>());
            men[2].SetPreferences(new List<IAllocated>());

            var expectedIsFinal = true;
            var actualIsFinal = _alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenNoListsProvided()
        {
            var men = new List<IToOneAllocated>
                { new NamedToOneAllocated("a"), new NamedToOneAllocated("b"), new NamedToOneAllocated("c") };
            var women = new List<IToOneAllocated>
                { new NamedToOneAllocated("A"), new NamedToOneAllocated("B"), new NamedToOneAllocated("C") };


            var allocation = new OneToOneAllocation(men, women);
            var expectedIsFinal = true;
            var actualIsFinal = _alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenListsAreEmptyAndThereAreSomePairs()
        {
            var men = new List<IToOneAllocated>
                { new NamedToOneAllocated("a"), new NamedToOneAllocated("b"), new NamedToOneAllocated("c") };
            var women = new List<IToOneAllocated>
                { new NamedToOneAllocated("A"), new NamedToOneAllocated("B"), new NamedToOneAllocated("C") };


            var allocation = new OneToOneAllocation(men, women);
            men[0].SetPreferences(new List<IAllocated> { women[0], women[1], women[2] });
            men[1].SetPreferences(new List<IAllocated> { women[1], women[2], women[0] });
            men[2].SetPreferences(new List<IAllocated> { women[2], women[0], women[1] });

            men[0].Assign(women[0]);
            men[1].Assign(women[1]);

            var expectedIsFinal = false;
            var actualIsFinal = _alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }

        [Test]
        public void testIsFinal_WhenListsAreNotEmptyAndThereAreSomePairs()
        {
            var men = new List<IToOneAllocated>
                { new NamedToOneAllocated("a"), new NamedToOneAllocated("b"), new NamedToOneAllocated("c") };
            var women = new List<IToOneAllocated>
                { new NamedToOneAllocated("A"), new NamedToOneAllocated("B"), new NamedToOneAllocated("C") };


            var allocation = new OneToOneAllocation(men, women);
            men[1].Assign(women[1]);
            men[0].Assign(women[0]);

            //Для последнего достижимых нет
            var expectedIsFinal = true;
            var actualIsFinal = _alg.isFinal(allocation);
            Assert.AreEqual(expectedIsFinal, actualIsFinal);
        }


        [Test]
        public void testIsFinal_WhenListsNotEmptyAndAllArePaired()
        {
            var men = new List<IToOneAllocated>
                { new NamedToOneAllocated("a"), new NamedToOneAllocated("b"), new NamedToOneAllocated("c") };
            var women = new List<IToOneAllocated>
                { new NamedToOneAllocated("A"), new NamedToOneAllocated("B"), new NamedToOneAllocated("C") };


            var allocation = new OneToOneAllocation(men, women);
            men[0].SetPreferences(new List<IAllocated> { women[0], women[1], women[2] });
            men[1].SetPreferences(new List<IAllocated> { women[1], women[2], women[0] });
            men[2].SetPreferences(new List<IAllocated> { women[2], women[0], women[1] });

            men[0].Assign(women[0]);
            men[1].Assign(women[1]);
            men[2].Assign(women[2]);

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
            var men = new List<IToOneAllocated>
                { new NamedToOneAllocated("a"), new NamedToOneAllocated("b"), new NamedToOneAllocated("c") };
            var women = new List<IToOneAllocated>
                { new NamedToOneAllocated("A"), new NamedToOneAllocated("B"), new NamedToOneAllocated("C") };


            var allocation = new OneToOneAllocation(men, women);
            men[0].SetPreferences(new List<IAllocated> { women[0], women[1], women[2] }); //A B C
            men[1].SetPreferences(new List<IAllocated> { women[1], women[2], women[0] }); //B C A
            men[2].SetPreferences(new List<IAllocated> { women[2], women[0], women[1] }); //C A B

            women[0].SetPreferences(new List<IAllocated> { men[0] }); //a
            women[1].SetPreferences(new List<IAllocated> { men[1] }); //b
            women[2].SetPreferences(new List<IAllocated> { men[2] }); //c


            _alg.computeIteration(allocation);

            var result = allocation.GetAllocationResult();
            var resultString = PrintUtilsV2.ToString2(result);

            Console.WriteLine(resultString);
            // all assigned to first person in list
            //should be Aa Bb Cc
            Assert.AreEqual("[a:A],[b:B],[c:C],", resultString);
        }

        [Test]
        public void WhenSomeStaySingle()
        {
            var men = new List<IToOneAllocated>
                { new NamedToOneAllocated("a"), new NamedToOneAllocated("b"), new NamedToOneAllocated("c") };
            var women = new List<IToOneAllocated>
                { new NamedToOneAllocated("A"), new NamedToOneAllocated("B"), new NamedToOneAllocated("C") };


            var allocation = new OneToOneAllocation(men, women);
            men[0].SetPreferences(new List<IAllocated> { women[0], women[1], women[2] }); //A B C
            men[1].SetPreferences(new List<IAllocated> { women[0], women[2] }); //A C
            men[2].SetPreferences(new List<IAllocated> { women[2], women[0], women[1] }); //C A B

            women[0].SetPreferences(new List<IAllocated> { men[0] }); //a
            women[1].SetPreferences(new List<IAllocated> { men[0] }); //a
            women[2].SetPreferences(new List<IAllocated> { men[2], men[1] }); //c b

            _alg.computeIteration(allocation);

            var result = allocation.GetAllocationResult();
            var resultString = PrintUtilsV2.ToString2(result);

            Console.WriteLine(resultString);
            // all assigned to first person in list
            //should be AA CC B_ _B 
            Assert.AreEqual("[a:A],[b:],[c:C],[:B],", resultString);
        }
    }
    
    
    [TestFixture]
    public class IterationsTillFinalTests : DaaTests
    {
        [Test]
        public void WhenSomeStaySingle()
        {
            // Создаем мужчин
            var M1 = new NamedToOneAllocated("M1");
            var M2 = new NamedToOneAllocated("M2");
            var M3 = new NamedToOneAllocated("M3");
            var M4 = new NamedToOneAllocated("M4");

            // Создаем женщин
            var W1 = new NamedToOneAllocated("W1");
            var W2 = new NamedToOneAllocated("W2");
            var W3 = new NamedToOneAllocated("W3");
            var W4 = new NamedToOneAllocated("W4");

     
            // Предпочтения 
            M1.SetPreferences(new List<IAllocated> { W2 });
            M2.SetPreferences(new List<IAllocated> {  W1, W2, W3  });
            M3.SetPreferences(new List<IAllocated> {  W1, W2, W3  });
  
            W1.SetPreferences(new List<IAllocated> { M1, M2, M3 });
            W2.SetPreferences(new List<IAllocated> { M2, M3 });
            W3.SetPreferences(new List<IAllocated> { M1, M2, M3 });
            
            // Списки мужчин и женщин

            var men = new List<IToOneAllocated> { M1, M2, M3 };
            var women = new List<IToOneAllocated> { W1, W2, W3 };


            var allocation = new OneToOneAllocation(men, women);
            while(!_alg.isFinal(allocation))
                _alg.computeIteration(allocation);

            var result = allocation.GetAllocationResult();
            var resultString = PrintUtilsV2.ToString2(result);

            Console.WriteLine(resultString);
            // M1 и M2 останутся не распределенными
            Assert.AreEqual("[M1:],[M2:W1],[M3:W2],[:W3],", resultString);
        }
    }
}