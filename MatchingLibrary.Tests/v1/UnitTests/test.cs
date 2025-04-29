using MatchingLibrary.v1.Algorithms.impl.Smp;
using MatchingLibrary.v1.Allocated.Impl;
using MatchingLibrary.v1.Allocation.impl.Smp;
using NUnit.Framework;

namespace MatchingLibrary.Tests.v1.UnitTests;

[TestFixture]
public class Test
{
    [Test]
    public void Main()
    {
        // Создаем мужчин
        var m1 = new NamedAllocated("M1");
        var m2 = new NamedAllocated("M2");
        var m3 = new NamedAllocated("M3");
        var m4 = new NamedAllocated("M4");

        // Создаем женщин
        var w1 = new NamedAllocated("W1");
        var w2 = new NamedAllocated("W2");
        var w3 = new NamedAllocated("W3");
        var w4 = new NamedAllocated("W4");

        // Списки мужчин и женщин
        var men = new List<NamedAllocated> { m1, m2, m3 };
        var women = new List<NamedAllocated> { w1, w2, w3 };


        // Предпочтения мужчин
        var menPreferences = new Dictionary<NamedAllocated, List<NamedAllocated>>
        {
            { m1, new List<NamedAllocated> { w2 } },
            { m2, new List<NamedAllocated> { w1, w2, w3 } },
            { m3, new List<NamedAllocated> { w1, w2, w3 } }
        };

        // Предпочтения женщин
        var womenPreferences = new Dictionary<NamedAllocated, List<NamedAllocated>>
        {
            { w1, new List<NamedAllocated> { m1, m2, m3 } },
            { w2, new List<NamedAllocated> { m2, m3 } },
            { w3, new List<NamedAllocated> { m1, m2, m3 } }
        };

        var allocation = new SmpAllocationGeneric<NamedAllocated, NamedAllocated>(men, women);

        var alg = new DAAAlgorithm<NamedAllocated, NamedAllocated?>();
        while (!alg.isFinal(allocation))
            alg.computeIteration(allocation);
        Console.WriteLine(allocation.ToString());
    }
}