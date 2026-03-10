namespace cs210_assignment1;

public abstract class Calculator
{
    protected readonly Dictionary<string, double> _memory = new();
    
    protected Lexer GetLexer(string expression)
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

                while (i < expression.Length)
                {
                    if (char.IsDigit(expression[i]) || expression[i] == ',')
                    {
                        number += expression[i]; 
                    }
                    else if (expression[i] == '.')
                    {
                        number += ",";
                    }
                    else
                    {
                        break;
                    }
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
    public abstract void Run();
}