namespace MatchingLibrary.v2.Allocated;

public class ComplexOneToOneAllocated : BaseOneToOneAllocated, IEquatable<ComplexOneToOneAllocated>
{
    public ComplexOneToOneAllocated(string name)
    {
        this.name = name;
    }

    private string name { get; set; }

    public override string ToString()
    {
        return $"{name}";
    }
    
    
    public bool Equals(ComplexOneToOneAllocated? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return name == other.name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ComplexOneToOneAllocated)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(name);
    }
}