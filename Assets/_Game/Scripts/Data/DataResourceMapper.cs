using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataResourceMapper : Sirenix.OdinInspector.SerializedMonoBehaviour
{
    public static DataResourceMapper Instance;

    private void Awake(){
        Instance = this;
    }
    [SerializeField] private Dictionary<int, Sprite> discResourceMap;

    public Sprite GetDiscResource(int id){
        return discResourceMap[id];
    }
}
