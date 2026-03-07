namespace cs210_assignment1;

public class VariableNode(string name) : INode
{
    public string Name { get; set; } = name;

    public double Calculate()
    {
        throw new NotImplementedException();
    }
}