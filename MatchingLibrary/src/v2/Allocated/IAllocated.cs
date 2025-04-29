using System.Collections.Generic;

namespace MatchingLibrary.v2.Allocated;

public interface IAllocated
{
    public object? GetAssigned();
    public void Assign(IAllocated? assigned);
    public void breakAssigment(IAllocated? assigned);
    public List<IAllocated> GetPreferences();
    public void SetPreferences(List<IAllocated> preferences);
}