using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : class
{
    private readonly Stack<T> objects = new Stack<T>();
    private readonly System.Func<T> createFunc;
    private readonly System.Action<T> actionOnGet;
    private readonly System.Action<T> actionOnRelease;
    private readonly int maxSize;

    public ObjectPool(System.Func<T> createFunc, System.Action<T> actionOnGet, System.Action<T> actionOnRelease, int maxSize = 50)
    {
        this.createFunc = createFunc;
        this.actionOnGet = actionOnGet;
        this.actionOnRelease = actionOnRelease;
        this.maxSize = maxSize;
    }

    public T Get()
    {
        T obj = objects.Count > 0 ? objects.Pop() : createFunc();
        actionOnGet?.Invoke(obj);
        return obj;
    }

    public void Release(T obj)
    {
        if (objects.Count < maxSize)
        {
            actionOnRelease?.Invoke(obj);
            objects.Push(obj);
        }
        else
        {
            // Optional: Handle cases where the pool is full.
            if (obj is MonoBehaviour mb)
                Object.Destroy(mb.gameObject);
        }
    }
}

