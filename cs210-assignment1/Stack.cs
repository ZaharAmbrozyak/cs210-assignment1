namespace cs210_assignment1;

public class Stack<T>(int size)
{
    private T[] _array = new T[size];

    private int _pointer;

    public int Size => _pointer;

    public T Head =>
        _pointer == 0 ? throw new Exception("Tried to get head, but stack is empty!") : _array[_pointer - 1];

    public bool Empty => _pointer == 0;

    public void Add(T value)
    {
        if (_pointer == _array.Length)
        {
            throw new IndexOutOfRangeException($"Tried to add {value}, but stack is overflowed!");
        }

        _array[_pointer] = value;
        _pointer++;
    }

    public T Get()
    {
        if (_pointer == 0)
        {
            throw new IndexOutOfRangeException("Tried to get value, but stack is empty!");
        }

        T value = _array[_pointer - 1];
        _pointer--;

        return value;
    }
}