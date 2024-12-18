using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;
    [SerializeField] private Disc disc;
    [SerializeField] private float followMoveSpd, followAngularSpd;

    private bool isFollowing;

    private void Awake() {
        Instance = this;
    }

    [Sirenix.OdinInspector.Button]
    private void Test(){
        Debug.Log(Physics.Raycast(transform.position, transform.forward, 1000));
    }


    public void Follow(){
        //if(!isFollowing) return;
        var pos = disc.CamFollowPosition;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * this.followMoveSpd);
        var targetRotation = Quaternion.LookRotation(disc.transform.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, followAngularSpd * Time.deltaTime);
        //transform.LookAt(target.transform, Vector3.up);
    }

    public void AdjustLookatTarget(Vector3 targetPos, float targetAngleX, float targetAngleY){
        
        transform.position = VectorUtils.CircularInterpolate(transform.position, targetPos, disc.transform.position, Vector3.up, Time.deltaTime * this.followMoveSpd);
        var currentAngleX = transform.eulerAngles.x;
        var currentAngleY = transform.eulerAngles.y;
        currentAngleX = currentAngleX > 180 ? currentAngleX - 360 : currentAngleX;
        currentAngleY = currentAngleY > 180 ? currentAngleY - 360 : currentAngleY;
        if(currentAngleY - targetAngleY > 180){
            targetAngleY = 360 + targetAngleY;
        }
        var currentRot = transform.eulerAngles;
        // currentRot.x = Mathf.Lerp(currentAngleX, targetAngleX, Time.deltaTime);
        // transform.eulerAngles = currentRot;
        transform.eulerAngles = transform.eulerAngles.Set(
            x: Mathf.Lerp(currentAngleX, targetAngleX, Time.deltaTime),
            y: Mathf.Lerp(currentAngleY, targetAngleY, Time.deltaTime)   
        );
        //transform.eulerAngles = transform.eulerAngles.Set(x: Mathf.Lerp(currentAngleX, targetRot, Time.deltaTime));
    }

    public void SetFollow(bool state){
        this.isFollowing = state;
    }

    public void SetTarget(Disc target){
        this.disc = target;
    }
}
