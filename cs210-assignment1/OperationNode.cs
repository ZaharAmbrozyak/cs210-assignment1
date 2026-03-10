namespace cs210_assignment1;

public class OperationNode(string op, ArrayList<INode> operands) : INode
{
    public string Operator { get; } = op;
    
    public readonly ArrayList<INode> Arguments = operands;

    public INode Calculate(Dictionary<string, INode> memory, ArrayList<string> visited)
    {
        if (Operator == "=")
        {
            if (Arguments.Get(0) is not VariableNode variableNode)
            {
                throw new ArgumentException("Lhs should be a variable!");
            }
            
            visited.Add("Enter");
            
            var rhs = Arguments.Get(1).Calculate(memory, visited);

            visited.Remove("Enter");
            
            memory[variableNode.Name] = rhs;

            return rhs.Calculate(memory, visited);
        }

        var first = Arguments.Get(0).Calculate(memory, visited);
        var second = Arguments.Get(1).Calculate(memory, visited);

        if (first is NumberNode leftNumber && second is NumberNode rightNumber)
        {
            var left = leftNumber.Value;
            var right = rightNumber.Value;
            var result = Operator switch
            {
                "+" => left + right,
                "-" => left - right,
                "*" => left * right,
                "/" => left / right,
                "^" => Math.Pow(left, right),
                _ => throw new ArgumentException($"Operator {Operator} does not exist!")
            };
            return new NumberNode(result);
        }

        var args = new ArrayList<INode>();
        args.Add(first);
        args.Add(second);
        return new OperationNode(Operator, args);
    }

    public string Show(Dictionary<string, INode> memory)
    {
        var first = Arguments.Get(0).Show(memory);
        var second = Arguments.Get(1).Show(memory);

        return first + Operator + second;
    }
}