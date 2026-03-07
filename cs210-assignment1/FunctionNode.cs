using System.Collections;

namespace cs210_assignment1;

public class FunctionNode(string name, ArrayList<INode> arguments) : INode
{
    public string Name { get; set; } = name;

    public ArrayList<INode> Arguments { get; } = arguments;

    public double Calculate()
    {
        throw new NotImplementedException();
    }
}