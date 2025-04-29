using System.Collections.Generic;

namespace MatchingLibrary.v2_OLD.Allocated;

public class BaseToOneAllocated : IToOneAllocated
{
    protected IAllocated? pair;
    protected List<IAllocated> preferences;

    protected BaseToOneAllocated()
    {
        preferences = new List<IAllocated>();
    }

    public IAllocated? GetAssigned()
    {
        return pair;
    }

    public void Assign(IAllocated? newPair)
    {
        pair = newPair;
    }

    object? IAllocated.GetAssigned()
    {
        return GetAssigned();
    }

    public List<IAllocated> GetPreferences()
    {
        return preferences;
    }

    public void SetPreferences(List<IAllocated> newPreferences)
    {
        preferences = newPreferences;
    }
}