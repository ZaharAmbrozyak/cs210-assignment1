namespace cs210_assignment1;

public class NumberToken(double value) : IToken
{
    public double Value { get; set; } = value;
}