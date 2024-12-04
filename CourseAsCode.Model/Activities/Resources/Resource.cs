namespace CourseAsCode.Model.Resources;

public abstract class Resource
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    // TODO: maybe change this to an enum?
    public string Type { get; set; }
}