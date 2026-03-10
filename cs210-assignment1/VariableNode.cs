namespace cs210_assignment1;

public class VariableNode(string name) : INode
{
    public string Name { get; } = name;

    public double Calculate(Dictionary<string, double> memory)
    {
        if (memory.TryGetValue(Name, out var value))
        {
            return value;
        }

        throw new ArgumentException($"Variable '{Name}' does not exist!");
    }
}