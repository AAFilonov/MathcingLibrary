using Newtonsoft.Json;

namespace MatchingLibrary.Allocated;

public class BaseToManyFloatAllocated : IToManyFloatAllocated
{
    protected List<IAllocated> _assigned;
    protected List<IAllocated> _preferences;

    [JsonProperty] protected double capacity;

    public BaseToManyFloatAllocated(string name, double capacity = 0)
    {
        this.name = name;
        _preferences = new List<IAllocated>();
        _assigned = new List<IAllocated>();
        this.capacity = capacity;
    }


    public string name { get; set; }

    public List<IAllocated> GetAssigned()
    {
        return _assigned;
    }

    public void Assign(IAllocated assigned)
    {
        _assigned.Add(assigned);
    }

    public void breakAssigment(IAllocated? assigned)
    {
        _assigned.Remove(assigned);
    }

    object? IAllocated.GetAssigned()
    {
        return GetAssigned();
    }

    public List<IAllocated> GetPreferences()
    {
        return _preferences;
    }

    public void SetPreferences(List<IAllocated> newPreferences)
    {
        _preferences = newPreferences;
    }

    public double GetCapacity()
    {
        return capacity;
    }

    public void SetCapacity(double newCapacity)
    {
        capacity = newCapacity;
    }


    public bool Equals(BaseToManyFloatAllocated? other)
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
        return Equals((BaseToManyFloatAllocated)obj);
    }

    public override int GetHashCode()
    {
        return name.GetHashCode();
    }
}