namespace cs210_assignment1;

public class FunctionToken(string name) : IToken
{
    public string Name { get; set; } = name;
}