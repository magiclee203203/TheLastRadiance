public class VariableData<T> : IVariableData
{
    private T _value;

    public VariableData(T value)
    {
        _value = value;
    }

    public bool TryGetValue<T1>(out T1 value)
    {
        if (_value is T1 v)
        {
            value = v;
            return true;
        }

        value = default;
        return false;
    }

    public bool TryStrictlyGetValue<T1>(out T1 value)
    {
        if (_value.GetType() == typeof(T1))
            return TryGetValue(out value);

        value = default;
        return false;
    }

    public void SetValue(T value)
    {
        _value = value;
    }
}