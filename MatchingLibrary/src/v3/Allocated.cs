namespace MatchingLibrary.v3;

public enum AllocatedType
{
    ToOneAllocated,
    ToManyAllocated,
    ToManyTwoStepAllocated,
}
public class Allocated<TData>
{
    public TData data { get; set; }
    private AllocatedType type;
    public int capacity;
    public List<object> assigned { get; set; }
    public List<object> preferences { get; set; }

    public List<T> GetAssignedData<T>()
    {
        List<T> listAssigned = new List<T>();
        foreach (Allocated<T> o in assigned)
        {
            listAssigned.Add((T) o.data);
        }

        return listAssigned;
    }
    public List<Allocated<T>> GetAssigned<T>()
    {
        List<Allocated<T>> listAssigned = new List<Allocated<T>>();
        for (var index = 0; index < assigned.Count; index++)
        {
            var o = (Allocated<T>) assigned[index];
            listAssigned.Add(o);
        }

        return listAssigned;
    }
    
    public Allocated(TData data, AllocatedType type, int capacity=1)
    {
        this.data = data;
        this.type = type;
        this.capacity = capacity;
        this.assigned = new List<object>() ;
        this.preferences = new List<object>() ;
    }
    //private List<object> projects;
}





