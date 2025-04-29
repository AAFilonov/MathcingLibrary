using System;

namespace MatchingLibrary.v2.Allocated.impl;

public class ComplexTwoStepAllocated : BaseTwoStepAllocated, IEquatable<ComplexTwoStepAllocated>
{
    public ComplexTwoStepAllocated(string name, int capasity = 0) : base(capasity)
    {
        this.name = name;
    }

    public string name { get; set; }


    public bool Equals(ComplexTwoStepAllocated? other)
    {
        return name == other.name;
    }

    public override string ToString()
    {
        return $"{name}";
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ComplexTwoStepAllocated) obj);
    }

    public override int GetHashCode()
    {
        return name.GetHashCode();
    }
}