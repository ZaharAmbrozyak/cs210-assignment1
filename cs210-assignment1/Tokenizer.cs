namespace cs210_assignment1;

public class Tokenizer
{
    protected Lexer GetLexer(string expression)
    {
        var lexer = new Lexer(expression.Length + 10);
        var i = 0;
        var expectMinus = true;
        while (i < expression.Length)
        {
            var symbol = expression[i];

            if (char.IsWhiteSpace(symbol))
            {
                i++;
            }
            else if (expectMinus && symbol == '-')
            {
                lexer.Add(new NumberToken(-1));
                lexer.Add(new OperationToken("*"));
                i++;
                expectMinus = false;
            }
            else if (char.IsDigit(symbol))
            {
                var number = "";

                while (i < expression.Length)
                {
                    if (char.IsDigit(expression[i]) || (expression[i] == ',' && char.IsDigit(expression[i + 1])))
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
                expectMinus = false;
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
                if (symbol == '(' || symbol == '=')
                {
                    expectMinus = true;
                }
                lexer.Add(new OperationToken(symbol.ToString()));
                i++;
            }
        }
        lexer.Add(new EofToken());
        return lexer;
    }
}