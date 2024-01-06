namespace MatchingLibrary.v2.Allocated;

public class BaseToManyAllocated : IToManyAllocated
{
    protected List<IAllocated> _assigned;
    protected int _capacity;
    protected List<IAllocated> _preferences;

    protected BaseToManyAllocated(int capacity)
    {
        _preferences = new List<IAllocated>();
        _assigned = new List<IAllocated>();
        _capacity = capacity;
    }

    public List<IAllocated> GetAssigned()
    {
        return _assigned;
    }

    public void Assign(IAllocated assigned)
    {
        _assigned.Add(assigned);
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
        return _capacity;
    }

    public void SetCapacity(int newCapacity)
    {
        _capacity = newCapacity;
    }

}