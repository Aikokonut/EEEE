using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiscAimer : MonoBehaviour
{
    [SerializeField] private float sensitivity;
    [SerializeField] private Transform camHolder;
    [SerializeField] private Vector2 verticalRotationLimit;
    [SerializeField] private Camera mainCam;

    public Vector3 Direction => this.transform.forward;


    private void Awake(){
        //mainCam = Camera.main;
    }

    public void Rotate(Vector2 dragVector){
        var angleX = dragVector.y;
        var angleY = dragVector.x;

        var currentRot = transform.eulerAngles;
        currentRot.y += angleY * sensitivity;

        if(currentRot.x > 180)
            currentRot.x -= 360;
        currentRot.x -= angleX * sensitivity;
        currentRot.x = Mathf.Clamp(currentRot.x, verticalRotationLimit.x, verticalRotationLimit.y);

        transform.eulerAngles = currentRot;
    }

    public void DetachCamera(){
        this.mainCam.transform.SetParent(null);
    }

    public void AttachCamera(){
        Debug.Log((this.mainCam, this.camHolder));
        this.mainCam.transform.SetPositionAndRotation(camHolder.position, camHolder.rotation);
        this.mainCam.transform.SetParent(this.transform);
    }
}
