using System.Collections;
using System.Collections.Generic;
using DL.StateMachine;
using UnityEngine;

public class PreThrowState : StateBehaviour
{
    public override void OnStateEnter(StateController stateController)
    {
        var throwController = stateController as ThrowStateController;

        UIManager.Instance.UIPreThrow.gameObject.SetActive(true);
        UIManager.Instance.UISessionComplete.gameObject.SetActive(false);
        UIManager.Instance.UILevelInfo.ShowUI();

        Debug.Log(throwController.Thrower);

        // throwController.Thrower.transform.position = LevelManager.Instance.CurrentSessionInfo.startInfo.position;
        // throwController.Aimer.transform.rotation = LevelManager.Instance.CurrentSessionInfo.startInfo.rotation;

        throwController.Thrower.Init();
        LevelManager.Instance.CurrentSessionInfo.sessionBound.UpdateDiscMarker();

    }

    public override void OnStateExit(StateController stateController)
    {
    }

    public override void OnStateFixedUpdate(StateController stateController)
    {
    }

    public override void OnStateUpdate(StateController stateController)
    {
    }
}
