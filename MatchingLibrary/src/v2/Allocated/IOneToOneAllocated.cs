namespace MatchingLibrary.v2.Allocated;

public interface IOneToOneAllocated
{
    public IOneToOneAllocated? GetAssigned();
    public void Assign(IOneToOneAllocated? pair);
    public List<IOneToOneAllocated> GetPreferences();
    public void SetPreferences(List<IOneToOneAllocated> preferences);
}