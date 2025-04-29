using MatchingLibrary.Allocated;

namespace MatchingLibrary.Utils;

public class AllocationUtils
{
    public static void createAssignment(IAllocated a, IAllocated b)
    {
        a.Assign(b);
        b.Assign(a);
    }

    public static void breakAssignment(IAllocated a, IAllocated b)
    {
        a.breakAssigment(b);
        b.breakAssigment(a);
    }
}