namespace Client;

public class Car
{
    private string? model;

    public int Id { get; set; }
    public string? Model { get => model; set => model = value; }
    public override string ToString()
    {
        return $"{Id} {Model}";
    }
}
