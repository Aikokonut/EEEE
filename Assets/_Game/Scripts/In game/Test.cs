using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL.Utils;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform testPoint;
    [SerializeField] private Transform testPlane;
    [SerializeField] private GameObject testobj;
    [Sirenix.OdinInspector.Button]
    private void TestIntersection(){
        Debug.Log(MathUtils.GetIntersectionWithPlane(this.testPoint.position, this.testPoint.forward, this.testPlane.up, this.testPlane.position));
    }

    private void Update(){
        if(Input.GetMouseButtonDown(0)){
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var point = MathUtils.GetIntersectionWithPlane(ray.origin, ray.direction, testPlane.up, testPlane.position);
            Instantiate(testobj, point, Quaternion.identity);
        }
    }
    [Sirenix.OdinInspector.Button]
    private void TestSpawn(){

    }
}
