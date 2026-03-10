using System.Collections;

namespace cs210_assignment1;

public class FunctionNode(string name, ArrayList<INode> arguments) : INode
{
    public string Name { get; } = name.ToLower();

    public ArrayList<INode> Arguments { get; } = arguments;

    public double Calculate(Dictionary<string, double> memory)
    {
        var calculatedArgs = new ArrayList<double>();

        for (var i = 0; i < Arguments.Size; i++)
        {
            var argument = Arguments.Get(i);
            calculatedArgs.Add(argument.Calculate(memory));
        }

        double argument1, argument2, result;
        switch (Name)
        {
            case "sin" or "cos" or "tan" or "log":
                if (Arguments.Size != 1)
                {
                    throw new ArgumentException($"Expected 1 argument but got {Arguments.Size}!");
                }
                argument1 = calculatedArgs.Get(0);
                
                result = Name switch
                {
                    "sin" => Math.Sin(argument1),
                    "cos" => Math.Cos(argument1),
                    "tan" => Math.Tan(argument1),
                    "log" => Math.Log(argument1)
                };
                break;
            case "max" or "min":
                if (Arguments.Size != 2)
                {
                    throw new ArgumentException($"Expected 2 arguments but got '{Arguments.Size}!");
                }
                
                argument1 = calculatedArgs.Get(0);
                argument2 = calculatedArgs.Get(1);

                result = Name switch
                {
                    "max" => Math.Max(argument1, argument2),
                    "min" => Math.Min(argument1, argument2)
                };
                break;
            default:
                throw new ArgumentException($"Function '{Name}' does not exist!");

        }

        return result;

    }
}