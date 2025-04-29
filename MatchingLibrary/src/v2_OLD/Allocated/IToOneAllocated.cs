namespace MatchingLibrary.v2_OLD.Allocated;

public interface IToOneAllocated : IAllocated
{
    public IAllocated? GetAssigned();
}