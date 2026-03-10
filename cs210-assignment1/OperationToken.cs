namespace cs210_assignment1;

public class OperationToken(string operation) : IToken
{
    public string Operation { get; } = operation;
}