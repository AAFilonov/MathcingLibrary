using System.Collections.Generic;

namespace MatchingLibrary.v2_OLD.Allocated;

public interface IToManyAllocated : IAllocated
{
    public List<IAllocated> GetAssigned();
    public int GetCapacity();
    public void SetCapacity(int newCapacity);
}