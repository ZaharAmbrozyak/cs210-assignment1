namespace cs210_assignment1;

public class Calculator
{
    public Lexer GetLexer(string expression)
    {
        var lexer = new Lexer(expression.Length + 1);
        var i = 0;
        while (i < expression.Length)
        {
            var symbol = expression[i];

            if (char.IsWhiteSpace(symbol))
            {
                i++;
            }
            else if (char.IsDigit(symbol))
            {
                var number = "";

                while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                {
                    number += expression[i];
                    i++;
                }

                lexer.Add(new NumberToken(double.Parse(number)));
            }
            else if (char.IsLetter(symbol))
            {
                var word = "";
                while (i < expression.Length && char.IsLetter(expression[i]))
                {
                    word += expression[i];
                    i++;
                }

                if (i < expression.Length && expression[i] == '(')
                {
                    lexer.Add(new FunctionToken(word));
                }
                else
                {
                    lexer.Add(new VariableToken(word));
                }
            }
            else
            {
                lexer.Add(new OperationToken(symbol.ToString()));
                i++;
            }
        }
        lexer.Add(new EofToken());
        return lexer;
    }

    public INode ParseExpression(Lexer lexer, double bindingPower)
    {
        var firstToken = lexer.Get();
        
        INode lhs;
        switch (firstToken)
        {
            case NumberToken numberToken:
                lhs = new NumberNode(numberToken.Value);
                break;
            case VariableToken variableToken:
                lhs = new VariableNode(variableToken.Name);
                break;
            case FunctionToken functionToken:
                if (lexer is { Peek: OperationToken { Operation: "(" } })
                {
                    lexer.Get();
                    var arguments = new ArrayList<INode>();

                    if (lexer is { Peek: OperationToken { Operation: ")" } })
                    {
                        lexer.Get();
                    }
                    else
                    {
                        while (true)
                        {
                            arguments.Add(ParseExpression(lexer, 0.0));

                            var nextToken = lexer.Get();
                            if (nextToken is OperationToken { Operation: "," })
                            {
                                continue;
                            }

                            if (nextToken is OperationToken { Operation: ")" })
                            {
                                break;
                            }

                            throw new ArgumentException($"Expected ',' or ')' but got {nextToken}");
                        }
                    }

                    lhs = new FunctionNode(functionToken.Name, arguments);
                }
                else
                {
                    throw new ArgumentException($"Expected '(' but got {lexer.Peek}");
                }

                break;
                
            case OperationToken { Operation: "(" }:
                lhs = ParseExpression(lexer, 0.0);
                
                if (lexer is not {Peek: OperationToken { Operation: ")" } })
                {
                    throw new ArgumentException($"Expected ')' but got {lexer.Peek}");
                }
                lexer.Get();
                break;
            default:
                throw new ArgumentException($"Unknown token: {firstToken}");
        }

        while (true)
        {
            var peekedToken = lexer.Peek;

            if (peekedToken is EofToken or OperationToken { Operation: ")" or "," })
            {
                break;
            }

            if (peekedToken is not OperationToken opToken)
            {
                throw new ArgumentException($"Wrong token: {peekedToken}");
            }

            var op = opToken.Operation;

            var (lBinding, rBinding) = GetBindingPower(op);

            if (lBinding < bindingPower)
            {
                break;
            }

            lexer.Get();

            var rhs = ParseExpression(lexer, rBinding);
            
            ArrayList<INode> operands = new();
            operands.Add(lhs);
            operands.Add(rhs);
            
            lhs = new OperationNode(op, operands);
        }

        return lhs;
    }

    public void ShowAst(INode node, string backTrack)
    {
        switch (node)
        {
            case NumberNode numberNode:
                Console.WriteLine(numberNode.Value);
                break;
            case OperationNode operationNode:
            {
                Console.WriteLine(operationNode.Operator);
                for (var i = 0; i < operationNode.Arguments.Size; i++)
                {
                    Console.Write(backTrack + "|-- ");
                
                    var isLast = (i == operationNode.Arguments.Size - 1); 
                    var nextBackTrack = backTrack + (isLast ? "    " : "|   ");
                    var operand = operationNode.Arguments.Get(i);
                
                    ShowAst(operand, nextBackTrack);
                }

                break;
            }
            case FunctionNode functionNode:
            {
                Console.WriteLine(functionNode.Name);
                for (var i = 0; i < functionNode.Arguments.Size; i++)
                {
                    Console.Write(backTrack + "|-- ");
                
                    var isLast = (i == functionNode.Arguments.Size - 1); 
                    var nextBackTrack = backTrack + (isLast ? "    " : "|   ");
                    var operand = functionNode.Arguments.Get(i);
                
                    ShowAst(operand, nextBackTrack);
                }

                break;
            }
        }
    }
    
    private (double, double) GetBindingPower(string op)
    {
        return op switch
        {
            "+" or "-" => (1.0, 1.1),
            "*" or "/" => (2.0, 2.1),
            "^" => (3.1, 3.0),
            _ => throw new ArgumentException($"Unknown operation: {op}")
        };
    }
}