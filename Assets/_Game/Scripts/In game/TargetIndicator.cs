using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    private Camera mainCam;

    private void Awake(){
        mainCam = Camera.main;
    }

    private void LateUpdate(){
        //transform.LookAt(mainCam.transform.position);
    }

    [Sirenix.OdinInspector.Button]
    private void Test(){
        foreach(var i in this.meshFilter.sharedMesh.vertices){
            Debug.Log(i);
        }
        foreach(var i in this.meshFilter.sharedMesh.triangles){
            Debug.Log(i);
        }
    }
}
