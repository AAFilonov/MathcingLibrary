namespace MatchingLibrary.v2.Allocated.impl;

public class ComplexToManyAllocated : BaseToManyAllocated, IEquatable<ComplexToManyAllocated>
{
    public ComplexToManyAllocated(string name, int capasity = 0) : base(capasity)
    {
        this.name = name;
    }

    public string name { get; set; }


    public bool Equals(ComplexToManyAllocated? other)
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
        return Equals((ComplexToManyAllocated) obj);
    }

    public override int GetHashCode()
    {
        return name.GetHashCode();
    }
}