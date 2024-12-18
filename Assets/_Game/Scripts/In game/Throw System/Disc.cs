using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Disc : MonoBehaviour
{
    [SerializeField] private float initialSpd, initialAngularSpd, dragAfterHit;
    

    private bool isFlying;
    private bool hit;
    private bool isOutOfBound;
    private float curlDir;

    private Vector3 flyingRightDir;
    private Vector3 currentAngularVel;
    private Vector3 currentVel;

    private Rigidbody body => this.GetComponent<Rigidbody>();

    public Vector3 CamFollowPosition => transform.position - this.currentVel.normalized * 2.75f + Vector3.up * 1f;
    public Vector3 VelocityBeforeHit => currentVel;

    public void StartDriveFlying(Vector3 direction, float curlDir, float throwStrength = 1){
        Debug.Log(throwStrength);
        body.velocity = direction * initialSpd * throwStrength;
        currentAngularVel = curlDir * initialAngularSpd * transform.up;
        body.useGravity = true;
        this.isFlying = true;
        this.curlDir = curlDir;

        this.flyingRightDir = transform.right;
    }
    public void StartPuttFlying(Vector3 velocity){
        this.transform.SetParent(null);
        body.velocity = velocity;
        body.useGravity = true;
        this.isFlying = true;
        this.curlDir = 0;
    }

    public void StopFlying(){
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.useGravity = false;
        this.isFlying = false;
        this.curlDir = 0;
        this.hit = false;
        this.body.drag = 0;
    }

    public void ResetState(){
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }

    private void Update(){
        if(this.isFlying && !hit){
            var speed = this.body.velocity.magnitude;
            this.body.velocity += curlDir * this.body.velocity.magnitude * Time.deltaTime * flyingRightDir;
            this.body.velocity = this.body.velocity.normalized * speed;
            this.body.angularVelocity = currentAngularVel;
            this.currentVel = body.velocity;
        }
    }

    public void Bend(float angle){
        if(this.isFlying) return;
        var rotation = transform.eulerAngles;
        rotation.z = angle;
        transform.eulerAngles = rotation;
    }

    private void OnCollisionEnter(Collision other) {
        if(!this.isFlying) return;
        this.body.drag = dragAfterHit;
        hit = true;
    }

    [Sirenix.OdinInspector.Button]
    private void Test(){
        Debug.Log(Singleton<WaterBox>.Instance.IsInsideWater(GetComponent<Collider>()));
    }


}
