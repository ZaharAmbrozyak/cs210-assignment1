namespace cs210_assignment1;

public class OperationNode(string op, ArrayList<INode> operands) : INode
{
    public string Operator { get; set; } = op;
    public ArrayList<INode> Operands = operands;

    public double Calculate()
    {
        throw new NotImplementedException();
    }
}