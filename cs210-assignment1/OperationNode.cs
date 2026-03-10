namespace cs210_assignment1;

public class OperationNode(string op, ArrayList<INode> operands) : INode
{
    public string Operator { get; } = op;
    public ArrayList<INode> Operands = operands;

    public double Calculate(Dictionary<string, double> memory)
    {
        if (Operator == "=")
        {
            if (Operands.Get(0) is not VariableNode variableNode)
            {
                throw new ArgumentException("Lhs should be a variable!");
            }

            var rhs = Operands.Get(1).Calculate(memory);

            memory[variableNode.Name] = rhs;

            return rhs;
        }

        var left = Operands.Get(0).Calculate(memory);
        var right = Operands.Get(1).Calculate(memory);

        return Operator switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => left / right,
            "^" => Math.Pow(left, right),
            _ => throw new ArgumentException($"Operator {Operator} does not exist!")
        };
    }
}