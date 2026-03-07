namespace cs210_assignment1;

public class Lexer(int size)
{
    private readonly Queue<IToken> _queue = new(size);
    
    public IToken Peek => _queue.Head;
    public int Size => _queue.Size;

    public IToken Get()
    {
        return _queue.Get();
    }
    public void Add(IToken token)
    {
        _queue.Add(token);
    }
}