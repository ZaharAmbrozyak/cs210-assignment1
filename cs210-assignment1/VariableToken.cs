namespace cs210_assignment1;

public class VariableToken(string name) : IToken
{
    public string Name { get; set; } = name;
}