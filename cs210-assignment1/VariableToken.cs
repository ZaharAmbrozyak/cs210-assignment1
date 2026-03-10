namespace cs210_assignment1;

public class VariableToken(string name) : IToken
{
    public string Name { get; } = name;

    public double Get(Dictionary<string, double> memory)
    {
        if (memory.TryGetValue(Name, out var value))
        {
            return value;
        }

        throw new KeyNotFoundException($"Variable {Name} does not exist!");
    }
}