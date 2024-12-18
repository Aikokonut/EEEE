using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DL.Utils;
using UnityEngine;

public class DiscPutter : DiscThrower
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private float releaseDist, spdMultiplier;
    [SerializeField] private GameObject dummyObj;
    private bool dragging;
    private Vector3 oldDiscPos;
    private Camera mainCam => CameraFollow.Instance.GetComponent<Camera>();

    private void Start()
    {
        oldDiscPos = this.Disc.transform.position;
    }

    [Sirenix.OdinInspector.Button]
    private void TestRaycast()
    {
        Debug.Log(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, 1000));
    }


    private void Update()
    {
        var ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(ray.origin, ray.direction, 1000, mask))
            {
                dragging = true;
                Debug.Log("asds");
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (dragging)
            {
                var dragPoint = MathUtils.GetIntersectionWithPlane(
                    ray.origin,
                    ray.direction,
                    this.Disc.transform.up,
                    ThrowStateController.Instance.Aimer.transform.position
                );
                this.Disc.transform.position = dragPoint;
                //dummyObj.transform.position = dragPoint;
                Debug.Log(dragPoint);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (dragging)
            {
                dragging = false;
                var vel = (this.Disc.transform.position - oldDiscPos) / Time.deltaTime;
                this.Disc.StartPuttFlying(vel);
            }
        }
        var distToOrigin = Vector3.Distance(this.Disc.transform.position, ThrowStateController.Instance.Aimer.transform.position);
        if (dragging && distToOrigin >= this.releaseDist)
        {
            // var vel = (this.Disc.transform.position - oldDiscPos) / Time.deltaTime * spdMultiplier;
            // Debug.Log(vel.magnitude);
            // this.Disc.StartPuttFlying(vel);
            // dragging = false;
            ThrowStateController.Instance.ChangeState("Flying");

        }
        this.oldDiscPos = this.Disc.transform.position;
    }

    public override void Throw()
    {
        var vel = (this.Disc.transform.position - oldDiscPos) / Time.deltaTime * spdMultiplier;
        Debug.Log(vel.magnitude);
        this.Disc.StartPuttFlying(vel);
        dragging = false;
    }
}
