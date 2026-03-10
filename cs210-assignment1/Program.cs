namespace cs210_assignment1;

static class Program
{
    private static void Main()
    {

        // var shuntingYard = new ShuntingYard();
        // shuntingYard.Run();

        var calculator = new Calculator();

        var expression = Console.ReadLine()!;
        var lexer = calculator.GetLexer(expression);
        var node = calculator.ParseExpression(lexer, 0.0);
        Dictionary<string, double> memory = new();
        Console.WriteLine(node.Calculate(memory));
        calculator.ShowAst(node, "");
        
    }
}