using System.Collections.Generic;

namespace MatchingLibrary.v3.exampleClasses;

public class Named
{
    protected  Named(string name)
    {
        this.name = name;
    }

    public string name { get; set; }

    public override string ToString()
    {
        return $"{name}";
    }
}
public class Student : Named, IAllocated<Student>
{
    public string preferenceString { get; set; }

    public Student(string name) : base(name)
    {
        
    }

    public Allocated<Student> GetAllocated()
    {
        Allocated<Student> allocatedStudent = new Allocated<Student>(this,AllocatedType.ToManyAllocated);
        return allocatedStudent;
    }
}
public class Tutor : Named, IAllocated<Tutor>
{
    public Tutor(string name) : base(name)
    {
        
    }

    public Allocated<Tutor> GetAllocated()
    {
        Allocated<Tutor> tutor = new Allocated<Tutor>(this,AllocatedType.ToManyAllocated, 2);
        tutor.preferences = new List<object>();
        return tutor;
    }
}
public class Project : Named
{
    public Project(string name) : base(name)
    {
        
    }
}
