namespace MatchingLibrary.v2.Allocated;

public interface IToOneAllocated : IAllocated
{
    public IAllocated? GetAssigned();
}