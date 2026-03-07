namespace cs210_assignment1;

using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

        var calculator = new Calculator();
        var expression = Console.ReadLine();
        
        var lexer = calculator.GetLexer(expression);

        var postfix = calculator.GetPostfix(lexer);
    }
}