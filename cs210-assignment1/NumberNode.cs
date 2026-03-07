namespace cs210_assignment1;

public class NumberNode(double value) : INode
{
    public double Value { get; set; } = value;

    public double Calculate()
    {
        return Value;
    }
}