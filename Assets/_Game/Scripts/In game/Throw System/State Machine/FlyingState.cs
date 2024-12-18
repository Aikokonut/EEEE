using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL.StateMachine;

public class FlyingState : StateBehaviour
{
    private float cd;
    public override void OnStateEnter(StateController stateController)
    {
        var throwController = stateController as ThrowStateController;
        UIManager.Instance.UIPreThrow.gameObject.SetActive(false);
        Debug.Log("drop");
        // throwController.Thrower.Throw();
        DiscSelector.Instance.SelectedThrower.Throw();
        UIManager.Instance.UILevelInfo.HideUI();
    }

    public override void OnStateExit(StateController stateController)
    {
        
    }

    public override void OnStateLateUpdate(StateController stateController)
    {
        if(DiscSelector.Instance.SelectedThrower.GetType() == typeof(DiscPutter)){
            return;
        }
        CameraFollow.Instance.Follow();
    }

    public override void OnStateUpdate(StateController stateController)
    {
        var throwController = stateController as ThrowStateController;
        var disc = throwController.Thrower.Disc;
        var discSpd = disc.GetComponent<Rigidbody>().velocity.magnitude;
        if(Mathf.Abs(discSpd) < 0.01f){
            cd += Time.deltaTime;
            if(cd > 2){
                throwController.ChangeState("After Throw");
            }
        }
        else{
            cd = 0;
        }

        LevelManager.Instance.CurrentSessionInfo.sessionBound.UpdateDiscMarker();
    }
}
