namespace cs210_assignment1;

public interface INode
{
    public INode Calculate(Dictionary<string, INode> memory, ArrayList<string> visited);

    public string Show(Dictionary<string, INode> memory);
}