using Newtonsoft.Json;

namespace MatchingLibrary.Allocated;

public class BaseToManyAllocated : IToManyAllocated
{
    protected List<IAllocated> _assigned;
    protected List<IAllocated> _preferences;

    [JsonProperty] protected int capacity;

    protected BaseToManyAllocated(int capacity)
    {
        _preferences = new List<IAllocated>();
        _assigned = new List<IAllocated>();
        this.capacity = capacity;
    }

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

    public int GetCapacity()
    {
        return capacity;
    }

    public void SetCapacity(int newCapacity)
    {
        capacity = newCapacity;
    }
}