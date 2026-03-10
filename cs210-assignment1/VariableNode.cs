namespace cs210_assignment1;

public class VariableNode(string name) : INode
{
    public string Name { get; } = name;

    public INode Calculate(Dictionary<string, INode> memory, ArrayList<string> visited)
    {
        if (visited.Find(Name))
        {
            throw new Exception($"Wrong operation! Cannot assign a=b and b=a !");
        }
        if (! memory.TryGetValue(Name, out var tree))
        {
            return this;
        }
        
        visited.Add(Name);
            
        var shortedTree = tree.Calculate(memory, visited);
            
        visited.Remove(Name);
            
        return shortedTree;

        
    }

    public string Show(Dictionary<string, INode> memory)
    {
        return Name;
    }
}