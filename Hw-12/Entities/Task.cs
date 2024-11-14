using Hw_12.Entities;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime TimeToDone { get; set; }
    public int Order { get; set; }
    public EnumState State { get; set; }
    public User User { get; set; }
    public int UserID { get; set; }

}

