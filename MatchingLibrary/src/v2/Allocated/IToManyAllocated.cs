namespace MatchingLibrary.v2.Allocated;

public interface IToManyAllocated : IAllocated
{
    public List<IAllocated> GetAssigned();
    public int GetCapacity();
    public void SetCapacity(int newCapacity);
}