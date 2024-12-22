public interface IVariableData
{
    bool TryGetValue<T>(out T value);
    bool TryStrictlyGetValue<T>(out T value);
}