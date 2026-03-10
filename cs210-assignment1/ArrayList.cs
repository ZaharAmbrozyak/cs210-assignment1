namespace cs210_assignment1;

public class ArrayList<T>
{
    private T[] _array = new T[1];
    private int _pointer;

    public int Size => _pointer;

    public void Add(T value)
    {
        if (_pointer == _array.Length)
        {
            var newArray = new T[_array.Length * 2];
            for (var i = 0; i < _pointer; i++)
            {
                newArray[i] = _array[i];
            }

            _array = newArray;
        }

        _array[_pointer] = value;
        _pointer++;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= _pointer)
        {
            throw new IndexOutOfRangeException("Index is out of range!");
        }

        return _array[index];
    }

    public void Delete(int index)
    {
        if (index < 0 || index >= _pointer)
        {
            throw new IndexOutOfRangeException("Index is out of range!");
        }

        for (var i = index; i < _pointer - 1; i++)
        {
            _array[i] = _array[i + 1];
        }

        _pointer--;
    }

    public void Remove(T value)
    {
        for(var i = 0; i < _pointer; i++)
        {
            if (Equals(_array[i], value))
            {
                Delete(i);
                return;
            }
        }

        throw new ArgumentException($"{value} was not found!");
    }
    public bool Find(T value)
    {
        for (var i = 0; i < _pointer; i++)
        {
            if (Equals(_array[i], value))
            {
                return true;
            }
        }

        return false;
    }
}