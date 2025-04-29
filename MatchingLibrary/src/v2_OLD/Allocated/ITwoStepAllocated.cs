using System.Collections.Generic;

namespace MatchingLibrary.v2_OLD.Allocated;

public interface ITwoStepAllocated : IToManyAllocated
{
    public List<IAllocated> GetProjects();
}