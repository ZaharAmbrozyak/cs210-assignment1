namespace cs210_assignment1;

public class ShuntingYard
{
    private readonly Dictionary<string, double> _memory = new();
    
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

    public PostfixExpression GetPostfix(Lexer lexer)
    {
        var postfix = new PostfixExpression(lexer.Size + 1);
        var stack = new Stack<IToken>(lexer.Size);

        while (lexer.Peek is not EofToken)
        {
            var token = lexer.Get();
            if (token is OperationToken { Operation: "=" })
            {
                if (postfix is {Size: 1, Peek: VariableToken variableToken})
                {
                    var variableName = variableToken.Name;
                    var rhsPostfix = GetPostfix(lexer);
                    var value = CalculatePostfix(rhsPostfix);
                    _memory[variableName] = value;
                    
                    rhsPostfix.Add(new NumberToken(value));
                    rhsPostfix.Add(new EofToken());
                    return rhsPostfix;
                }
                throw new Exception("Lhs should be a variable!");
            }
            else if (token is NumberToken numberToken)
            {
                postfix.Add(numberToken);
            }
            else if (token is VariableToken variableToken)
            {
                postfix.Add(variableToken);
            }
            else if(token is FunctionToken functionToken)
            {
                stack.Add(functionToken);
            }
            
            else if (token is OperationToken { Operation: "(" })
            {
                stack.Add(token);
            }
            else if (token is OperationToken { Operation: "," })
            {
                while (stack is { Empty: false, Head: not OperationToken { Operation: "(" } })
                {
                    postfix.Add(stack.Get());
                }
            }
            
            else if (token is OperationToken { Operation: ")" })
            {
                while (stack is { Empty: false, Head: OperationToken opToken } && opToken.Operation != "(" )
                {
                    postfix.Add(stack.Get());
                }

                if (stack is { Empty: false, Head: OperationToken { Operation: "(" } })
                {
                    stack.Get();
                }

                if (stack is { Empty: false, Head: FunctionToken })
                {
                    postfix.Add(stack.Get());
                }
            }
            else if (token is OperationToken operationToken)
            {
                while (stack is { Empty: false, Head: OperationToken stackToken } && stackToken.Operation != "(")
                {
                    var stackPriority = GetPriority(stackToken.Operation);
                    var tokenPriority = GetPriority(operationToken.Operation);

                    if ((operationToken.Operation == "^" && stackPriority > tokenPriority) ||
                        (operationToken.Operation != "^" && stackPriority >= tokenPriority))
                    {
                        postfix.Add(stack.Get());
                    }
                    else
                    {
                        break;
                    }
                }
                stack.Add(operationToken);
            }
            else
            {
                throw new ArgumentException($"Bad token: {token}");
            }
        }

        while (!stack.Empty)
        {
            var token = stack.Get();
            if (token is OperationToken { Operation: "(" })
            {
                continue;
            }
            
            postfix.Add(token);
        }
        postfix.Add(new EofToken());
        return postfix;
    }

    public double CalculatePostfix(PostfixExpression postfix)
    {
        var stack = new Stack<IToken>(postfix.Size); 
        double valueLeft, valueRight, result;
        while (postfix.Peek is not EofToken)
        {
            var token = postfix.Get();
            
            if (token is NumberToken)
            {
                stack.Add(token);
            }

            else if (token is VariableToken variableToken)
            {
                if(_memory.TryGetValue(variableToken.Name, out var value))
                {
                    stack.Add(new NumberToken(value));
                }
                else
                {
                    throw new KeyNotFoundException($"Variable '{variableToken.Name}' does not exist!");
                }
            }
            else if (token is OperationToken operationToken)
            {
                valueRight = ((NumberToken)stack.Get()).Value;

                valueLeft = ((NumberToken)stack.Get()).Value;

                result = operationToken.Operation switch
                {
                    "+" => valueLeft + valueRight,
                    "-" => valueLeft - valueRight,
                    "*" => valueLeft * valueRight,
                    "/" => valueLeft / valueRight,
                    "^" => Math.Pow(valueLeft, valueRight),
                    _ => throw new ArgumentException($"Operation '{operationToken.Operation}' does not exist!")
                };
                stack.Add(new NumberToken(result));
            }
            else if (token is FunctionToken functionToken)
            {
                    valueLeft = ((NumberToken)stack.Get()).Value;
                    
                    switch(functionToken.Name)
                    {
                        case "sin":
                            result = Math.Sin(valueLeft);
                            break;
                        case "cos":
                            result = Math.Cos(valueLeft);
                            break;
                            
                        case "tan":
                            result = Math.Tan(valueLeft);
                            break;
                            
                        case "log":
                            result = Math.Log(valueLeft);
                            break;
                        case "max":
                            valueRight = ((NumberToken)stack.Get()).Value;
                            result = Math.Max(valueLeft, valueRight);
                            break;
                        case "min":
                            valueRight = ((NumberToken)stack.Get()).Value;
                            result = Math.Min(valueLeft, valueRight);
                            break;
                        default:
                            throw new ArgumentException($"Operation '{functionToken.Name}' Does not exist!");
                    }
                    stack.Add(new NumberToken(result));
            }
        }

        postfix.Get();
        return ((NumberToken)stack.Get()).Value;
    }

    private double CalculateFromString(string expression)
    {
        var lexer = GetLexer(expression!);
        var postfix = GetPostfix(lexer);
        var result = CalculatePostfix(postfix);
        return result;
    }
    public void Run()
    {
        while (true)
        {
            Console.Write(">>> ");
            var expression = Console.ReadLine();
            if (string.IsNullOrEmpty(expression) || expression == "exit")
            {
                break;
            }
            Console.WriteLine("<<< " + CalculateFromString(expression));
        }

    }
    private double GetPriority(string op)
    {
        return op switch
        {
            "+" or "-" => 1,
            "*" or "/" => 2,
            "^" => 3,
            "sin" or "cos" or "tan" or "log" or "max" or "min" => 4,
            _ => throw new Exception($"Unknown operator: {op}")
        };
    }
}
