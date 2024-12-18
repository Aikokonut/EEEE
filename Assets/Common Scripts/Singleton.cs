using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: UnityEngine.Object
{
    public static T Instance => instanceHolder.instance as T;

    private Object instance;
    private static Singleton<T> instanceHolder;

    protected virtual void Awake(){
        instanceHolder = this;
        instanceHolder.instance = this;
    }
}
