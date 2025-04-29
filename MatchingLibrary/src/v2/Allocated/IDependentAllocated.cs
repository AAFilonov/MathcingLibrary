namespace MatchingLibrary.v2.Allocated;

public interface IDependentAllocated : IToManyAllocated
{
    public ITwoStepAllocated? GetOwner();
}