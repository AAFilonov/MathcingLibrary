namespace MatchingLibrary.Allocated;

public interface IToOneFloatAllocated : IAllocated
{
    public IAllocated? GetAssigned();
    public double GetRequestedQuota();
}