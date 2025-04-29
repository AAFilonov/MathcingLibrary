namespace MatchingLibrary.v2_OLD.Allocated;

public interface IDependentAllocated : IToManyAllocated
{
    public IAllocated? GetOwner();
}