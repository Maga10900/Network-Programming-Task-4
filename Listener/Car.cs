namespace Listener;

public class Car
{
    public int Id { get; set; }
    public string Model { get; set; }
    public override string ToString()
    {
        return $"{Id} {Model}";
    }
}
