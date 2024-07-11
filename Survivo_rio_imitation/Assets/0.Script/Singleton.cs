using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                T[] obj = GameObject.FindObjectsOfType<T>();
                if(obj.Length > 0)
                {
                    instance = obj[0];
                    GameObject.DontDestroyOnLoad(obj[0]);
                    for (int i = 1; i < obj.Length; i++)
                    {
                        GameObject.Destroy(obj[i]);
                    }
                }
                else
                {
                    GameObject newT = new GameObject(typeof(T).Name);
                    instance = newT.AddComponent<T>();
                    GameObject.DontDestroyOnLoad(newT);
                }                
            }
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    public void SettingInstance()
    {
        if (instance == null)
        {
            T[] obj = GameObject.FindObjectsOfType<T>();
            if (obj.Length > 0)
            {
                instance = obj[0];
                GameObject.DontDestroyOnLoad(obj[0]);
                for (int i = 1; i < obj.Length; i++)
                {
                    GameObject.Destroy(obj[i]);
                }
            }
            else
            {
                GameObject newT = new GameObject(typeof(T).Name);
                instance = newT.AddComponent<T>();
                GameObject.DontDestroyOnLoad(newT);
            }
        }
    }

    public virtual void Init()
    {

    }
}
