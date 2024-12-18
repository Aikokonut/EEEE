using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] protected List<UIComponent> componentList;

    protected Dictionary<Type, UIComponent> componentHashMap;

    public UIComponent GetUIComponent<T>() where T: UIComponent{
        var type = typeof(T);
        if(!componentHashMap.ContainsKey(type)) return null;
        return componentHashMap[type]; 
    }

    public virtual void Show(){

    }
    public virtual void Hide(){
        
    }
    public virtual void Disable(){
        
    }
}
