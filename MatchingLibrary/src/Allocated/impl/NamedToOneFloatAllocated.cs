namespace MatchingLibrary.Allocated.impl;

public class NamedToOneFloatAllocated : BaseToOneAllocated, IEquatable<NamedToOneAllocated>
{
    public NamedToOneFloatAllocated(string name, double quota)
    {
        this.name = name;
        this.quota = quota;
    }

    public string name { get; set; }
    public double quota { get; set; }


    public bool Equals(NamedToOneAllocated? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return name == other.name;
    }

    public override string ToString()
    {
        return $"{name}({quota})";
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((NamedToOneAllocated)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(name);
    }
}