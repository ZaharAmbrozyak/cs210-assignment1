using System.Collections;

namespace cs210_assignment1;

public class FunctionNode(string name, ArrayList<INode> arguments) : INode
{
    public string Name { get; } = name.ToLower();

    public ArrayList<INode> Arguments { get; } = arguments;

    public INode Calculate(Dictionary<string, INode> memory, ArrayList<string> visited)
    {
        var calculatedArgs = new ArrayList<INode>();

        for (var i = 0; i < Arguments.Size; i++)
        {
            var argument = Arguments.Get(i);
            calculatedArgs.Add(argument.Calculate(memory, visited));
        }

        double argument1, argument2, result;
        switch (Name)
        {
            case "sin" or "cos" or "tan" or "tg" or "log":
                if (Arguments.Size != 1)
                {
                    throw new ArgumentException($"Expected 1 argument but got {Arguments.Size}!");
                }

                if (calculatedArgs.Get(0) is not NumberNode numberNode)
                {
                    var args = new ArrayList<INode>();
                    args.Add(calculatedArgs.Get(0));
                    
                    return new FunctionNode(Name, args);
                }
                
                argument1 = numberNode.Value;
                
                result = Name switch
                {
                    "sin" => Math.Sin(argument1),
                    "cos" => Math.Cos(argument1),
                    "tan" or "tg" => Math.Tan(argument1),
                    "log" => Math.Log(argument1)
                };
                break;
            case "max" or "min":
                if (Arguments.Size != 2)
                {
                    throw new ArgumentException($"Expected 2 arguments but got {Arguments.Size}!");
                }

                if (calculatedArgs.Get(0) is not NumberNode leftNode ||
                    calculatedArgs.Get(1) is not NumberNode rightNode)
                {
                    var args = new ArrayList<INode>();
                    args.Add(calculatedArgs.Get(0));
                    args.Add(calculatedArgs.Get(1));
                    return new FunctionNode(Name, args);
                }
                
                argument1 = leftNode.Value;
                argument2 = rightNode.Value;

                result = Name switch
                {
                    "max" => Math.Max(argument1, argument2),
                    "min" => Math.Min(argument1, argument2)
                };
                break;
            default:
                throw new ArgumentException($"Function '{Name}' does not exist!");

        }

        return new NumberNode(result);
    }

    public string Show(Dictionary<string, INode> memory)
    {
        var arguments = "";
        for(var i = 0; i < Arguments.Size - 1; i++)
        {
            arguments += Arguments.Get(i).Show(memory) + ", ";
        }

        arguments += Arguments.Get(Arguments.Size - 1).Show(memory);
        return Name + "(" + arguments + ")";
    }
}