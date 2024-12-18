using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldBound : Singleton<WorldBound>
{
    private BoxCollider box => GetComponent<BoxCollider>();
    private Vector3 center;
    protected override void Awake(){
        base.Awake();
        
        this.center = box.center + transform.position;
    }

    public bool IsInBound(Vector3 position){
        position -= center;
        var extents = box.size / 2;
        this.center = box.center + transform.position;
        return (
            position.x > -extents.x && position.x < extents.x && 
            position.y > -extents.y && position.y < extents.y && 
            position.z > -extents.z && position.z < extents.z
        );
    }

    [Sirenix.OdinInspector.Button]
    private void Test(Transform testTransform){
        Debug.Log(IsInBound(testTransform.position));
    } 
}
