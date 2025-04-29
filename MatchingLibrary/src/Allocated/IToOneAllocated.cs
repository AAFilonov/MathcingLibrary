namespace MatchingLibrary.Allocated;

public interface IToOneAllocated : IAllocated
{
    public IAllocated? GetAssigned();
}