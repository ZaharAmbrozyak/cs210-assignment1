namespace cs210_assignment1;

public class NumberNode(double value) : INode
{
    public double Value { get; } = value;

    public INode Calculate(Dictionary<string, INode> memory)
    {
        return this;
    }

    public string Show(Dictionary<string, INode> memory)
    {
        return Value.ToString();
    }
}