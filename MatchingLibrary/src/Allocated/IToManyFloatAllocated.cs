namespace MatchingLibrary.Allocated;

public interface IToManyFloatAllocated : IAllocated
{
    public List<IAllocated> GetAssigned();

    public void Assign(IAllocated assigned);

    public double GetCapacity();
    public void SetCapacity(double newCapacity);
}