namespace cs210_assignment1;

public class VariableNode(string name) : INode
{
    public string Name { get; } = name;

    public INode Calculate(Dictionary<string, INode> memory)
    {
        if (memory.TryGetValue(Name, out var tree))
        {
            var shortedTree = tree.Calculate(memory);

            return shortedTree;
        }

        return this;
    }

    public string Show(Dictionary<string, INode> memory)
    {
        return Name;
    }
}