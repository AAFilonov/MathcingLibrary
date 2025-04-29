namespace MatchingLibrary.Allocated;

public interface IDependentAllocated : IToManyAllocated
{
    public ITwoStepAllocated? GetOwner();
}