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

                if (expression[i] == '(')
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
        var postfix = new PostfixExpression(lexer.Size);
        var stack = new Stack<IToken>(lexer.Size);

        while (lexer.Peek is not EofToken)
        {
            var token = lexer.Get();
            if (token is NumberToken numberToken)
            {
                postfix.Add(numberToken);
            }
            else if(token is FunctionToken functionToken)
            {
                postfix.Add(functionToken);
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
            else if (token is OperationToken { Operation: "," })
            {
                while (!stack.Empty && stack.Head is not OperationToken { Operation: "(" } token1)
                {
                    postfix.Add(stack.Get());
                }
            }
            else if (token is OperationToken { Operation: "(" })
            {
                stack.Add(token);
            }
            else if (token is OperationToken { Operation: ")" })
            {
                while (stack is { Empty: false, Head: OperationToken { Operation: "(" } })
                {
                    postfix.Add(stack.Get());
                }

                if (stack is { Empty: false, Head: OperationToken { Operation: "(" } })
                {
                    stack.Get();
                }

                if (stack is { Empty: false, Head: FunctionToken funcToken })
                {
                    postfix.Add(stack.Get());
                }
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
