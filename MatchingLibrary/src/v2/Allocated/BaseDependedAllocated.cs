namespace MatchingLibrary.v2.Allocated;

public class BaseDependedAllocated : BaseToManyAllocated, IDependentAllocated
{
    private ITwoStepAllocated? owner;
   
    public BaseDependedAllocated(int capacity) : base(capacity)
    {
    }

    public ITwoStepAllocated? GetOwner()
    {
        return owner;
    }

    public void SetOwner(ITwoStepAllocated newOwner)
    {
        owner = newOwner;
    }
}