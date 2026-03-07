namespace cs210_assignment1;

public class PostfixExpression(int size)
{
    private readonly Queue<IToken> _queue = new Queue<IToken>(size);

    public void Add(IToken token)
    {
        _queue.Add(token);
    }

    public IToken Get()
    {
        return _queue.Get();
    }
}