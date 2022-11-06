using UnityEngine;

[System.Serializable]
public class Twins<T>
{
    [SerializeField] private T value1;
    [SerializeField] private T value2;

    public void SetValue1(T value)
    {
        value1 = value;
    }

    public void SetValue2(T value)
    {
        value2 = value;
    }

    public T[] GetValue()
    {
        return new T[] { value1, value2 };
    }
}