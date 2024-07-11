using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject _value;

    public Queue< GameObject> _values = new Queue< GameObject >();
    public GameObject _valuePoint;

    public ObjectPool( GameObject value )
    {
        _value = value;
        _valuePoint = new GameObject();
        _valuePoint.gameObject.name = "[" + _value.name + "]";
        CreateValue(20);
    }

    public GameObject GetValue()
    {
        if(_values.Count <= 0)
        {
            CreateValue(10);
        }        

        GameObject obj = _values.Dequeue();        
        obj.gameObject.SetActive(true);

        return obj;
    }
   
    public void CreateValue(int Num = 5)
    {
        for (int i = 0; i < Num; i++)
        {
            GameObject obj = GameObject.Instantiate(_value);
            obj.transform.SetParent(_valuePoint.transform);
            obj.SetActive(false);
            _values.Enqueue( obj );
        }
    }

    public void ReturnObj( GameObject obj )
    {
        obj.SetActive(false);
        _values.Enqueue( obj );
    }
}
