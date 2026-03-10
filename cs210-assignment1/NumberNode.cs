namespace cs210_assignment1;

public class NumberNode(double value) : INode
{
    public double Value { get; } = value;

    public double Calculate(Dictionary<string, double> memory)
    {
        return Value;
    }
}