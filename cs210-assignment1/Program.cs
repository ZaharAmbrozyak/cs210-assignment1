namespace cs210_assignment1;

using System.Globalization;

static class Program
{
    private static void Main()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

        var shuntingYard = new ShuntingYard();
        
        // shuntingYard.Run();

        var calculator = new Calculator();

        var expression = Console.ReadLine()!;
        var lexer = calculator.GetLexer(expression);
        var node = calculator.ParseExpression(lexer, 0.0);
        calculator.ShowAST(node, "");
    }
}