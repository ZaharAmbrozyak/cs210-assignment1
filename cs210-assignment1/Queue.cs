namespace cs210_assignment1;

public class Queue<T>(int capacity)
{
    private readonly T[] _array = new T[capacity];
    private int _pointer;
    
    public int Size => _pointer;

    public T Head => _pointer == 0 ? throw new Exception("Tried to get head, but queue is empty!") : _array[0];

    public bool Empty => _pointer == 0;

    public void Add(T value)
    {
        if (_pointer == _array.Length)
        {
            throw new IndexOutOfRangeException($"Failed to add {value}. Queue is overflowed!");
        }

        _array[_pointer] = value;
        _pointer++;
    }

    public T Get()
    {
        if (_pointer == 0)
        {
            throw new Exception("Tried to get element, but queue is empty!");
        }

        var value = _array[0];
        for (var i = 0; i < _pointer - 1; i++)
        {
            _array[i] = _array[i + 1];
        }

        _pointer--;

        return value;
    }
    
}