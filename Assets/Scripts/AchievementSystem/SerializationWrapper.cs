using System.Collections.Generic;

[System.Serializable]
public class SerializationWrapper<T>
{
    public List<T> items;

    public SerializationWrapper(List<T> items)
    {
        this.items = items;
    }
}