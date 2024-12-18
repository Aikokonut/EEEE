using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscDriver : DiscThrower
{
    private void Start() {
        UIManager.Instance.UIPreThrow.UIBender.RegisterOnDragCallback(this.UIBendDragCallback);
        UIManager.Instance.UIPreThrow.UIBender.RegisterOnDropCallback(this.UIBendDropCallback);
    }
    public override void Throw()
    {
        this.Disc.transform.SetParent(null);
        this.Disc.StartDriveFlying(aimer.Direction, this.curl, this.throwStrength);
        this.aimer.DetachCamera();
        this.cameraFollow.SetFollow(true);
    }

    private void UIBendDragCallback(float dragLength, float hValue){
        this.curl = -hValue / dragLength;
        this.Disc.Bend(Mathf.Asin(hValue / dragLength) * Mathf.Rad2Deg);
        this.throwStrength = dragLength / 265;
    }
    private void UIBendDropCallback(float length, bool canRelease){
        if(canRelease) ThrowStateController.Instance.ChangeState("Flying");
        else this.Disc.transform.localEulerAngles = Vector3.zero;
    }

    private void OnDestroy() {
        UIManager.Instance.UIPreThrow.UIBender.UnregisterOnDragCallback(this.UIBendDragCallback);
        UIManager.Instance.UIPreThrow.UIBender.UnregisterOnDropCallback(this.UIBendDropCallback);
    }
}
