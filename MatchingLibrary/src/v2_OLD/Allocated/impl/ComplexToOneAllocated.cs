namespace MatchingLibrary.v2_OLD.Allocated.impl;

public class ComplexToOneAllocated : BaseToOneAllocated, IEquatable<ComplexToOneAllocated>
{
    public ComplexToOneAllocated(string name)
    {
        this.name = name;
    }

    public string name { get; set; }


    public bool Equals(ComplexToOneAllocated? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
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
        return Equals((ComplexToOneAllocated) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(name);
    }
}