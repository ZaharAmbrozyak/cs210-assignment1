namespace cs210_assignment1;

public class Calculator
{
    public Queue<IToken> GetLexer(string expression)
    {
        var lexer = new Queue<IToken>(expression.Length);
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

        return lexer;
    }
    
}
