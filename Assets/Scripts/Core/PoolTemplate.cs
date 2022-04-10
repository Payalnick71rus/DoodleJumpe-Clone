using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class PoolTemplate<T> where T : MonoBehaviour
{
    public event System.Action PoolIsReady;
    private List<T> _pool;
    private T _objectPrefab;
    private Transform _transform;
    private int _poolSize = 0;

    

    public PoolTemplate(int poolSize, T prefab, Transform container)
    {       
        _objectPrefab = prefab;
        _transform = container;
        _poolSize = poolSize;
        _pool = new List<T>();
    }


    private void InitStartPool(int size)
    {
        _pool.Clear();
        for (int i=0;i< size;i++)
        {
            _pool.Add(CreateObject());
        }
        PoolIsReady?.Invoke();
    }

    private T CreateObject(bool active = false)
    {       
        T obj = GameObject.Instantiate(_objectPrefab, _transform);
        obj.transform.position = Vector3.zero;
        obj.gameObject.SetActive(active);
        return obj;
    }
    public void InitPool()
    {
        InitStartPool(_poolSize);
    }
    public bool HasFreeObject(out T obj)
    {
        obj = null;
        foreach (T poolObj in _pool)
        {
            if(!poolObj.gameObject.activeSelf)
            {
                obj = poolObj;
                return true;
            }
        }
        return false;
    }

    public T GetObject()
    {
        T obj = null;
        if (HasFreeObject(out obj))
            return obj;
        obj = CreateObject();
        _pool.Add(obj);
        return obj;
    }

}
