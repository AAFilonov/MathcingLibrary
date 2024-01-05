namespace MatchingLibrary.v2.Allocated;

public class BaseOneToOneAllocated: IOneToOneAllocated
{
    protected  IOneToOneAllocated? pair;
    protected List<IOneToOneAllocated> preferences;

    protected BaseOneToOneAllocated()
    {
        preferences = new List<IOneToOneAllocated>();
    }
    
    public IOneToOneAllocated? GetAssigned()
    {
        return pair;
    }

    public void Assign(IOneToOneAllocated? newPair)
    {
        pair = newPair;
    }

    public List<IOneToOneAllocated> GetPreferences()
    {
        return preferences;
    }

    public void SetPreferences(List<IOneToOneAllocated> newPreferences)
    {
        preferences = newPreferences;
    }
    
    
}