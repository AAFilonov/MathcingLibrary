using MatchingLibrary.v2.Allocated;

namespace MatchingLibrary.v2;

public class AllocationUtils
{
    public static void createAssignment(IAllocated a, IAllocated b)
    {
        a.Assign(b);
        b.Assign(a);
    }

    public static void breakAssignment(IToOneAllocated a, IToOneAllocated b)
    {
        a.Assign(null);
        b.Assign(null);
    }

    public static void breakAssignment(IToManyAllocated a, IToOneAllocated b)
    {
        a.GetAssigned().Remove(b);
        b.Assign(null);
    }

    private static void breakAssignment(IToManyAllocated a, IToManyAllocated b)
    {
        a.GetAssigned().Remove(b);
        b.GetAssigned().Remove(a);
    }
}