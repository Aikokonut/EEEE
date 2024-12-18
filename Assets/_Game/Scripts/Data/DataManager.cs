using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    protected virtual void Awake(){
        Instance = this;
    }
    public abstract List<int> GetDiscData();
    public abstract void SetDiscData(List<int> discData);
}
