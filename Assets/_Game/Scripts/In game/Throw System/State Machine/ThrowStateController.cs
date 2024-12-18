using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL.StateMachine;
using Unity.VisualScripting;

public class ThrowStateController : StateController
{
    public static ThrowStateController Instance;
    [SerializeField] private DiscThrower driver, putter;
    [SerializeField] private DiscAimer aimer;
    [SerializeField] private float discMoveDist;
    private CameraFollow cameraFollow => CameraFollow.Instance;
    private UIManager uiManager => UIManager.Instance;

    private DiscThrower currentThrower;

    private void Start(){
        uiManager.UIAfterThrow.RegisterMoveToTieClick(this.MoveToTie);
        uiManager.UIAfterThrow.RegisterThrowAgainClick(this.ThrowAgain);
        uiManager.UISessionComplete.RegisterNextHoleClick(this.NextHole);
        uiManager.UIDirectionAdjuster.SetAimer(this.aimer);
        UIManager.Instance.UIMoveDisc.RegisterValueChangedCallback(this.MoveDiscHorizontally);
        base.Awake();

        var startPos = LevelManager.Instance.CurrentSessionInfo.startInfo.position;
        Thrower.transform.position = startPos;
        Aimer.transform.position = startPos;
        Aimer.transform.rotation = LevelManager.Instance.CurrentSessionInfo.startInfo.rotation;
        
    }


    protected override void Awake(){
        this.SetThrowerDrive();
        Instance = this;
        
    }
    

    public void ThrowAgain(){
        LevelManager.Instance.IncreaseThrow();
        this.ChangeState("Pre Throw");
    }
    public void MoveToTie(){        
        var newPos = Thrower.Disc.transform.position + Vector3.up;

        Debug.Log(Singleton<WaterBox>.Instance.IsInsideWater(Thrower.Disc.GetComponent<Collider>()));

        if(
            Singleton<WaterBox>.Instance.IsInsideWater(Thrower.Disc.GetComponent<Collider>()) ||
            !Singleton<WorldBound>.Instance.IsInBound(Thrower.Disc.transform.position)
        ){
            Debug.Log("out of bound");
            var point = Singleton<WaterBox>.Instance.GetClosestTerrainPoint(Thrower.Disc.transform.position);
            newPos = point + Vector3.up;
        }
        Thrower.transform.position = newPos;
        Aimer.transform.position = newPos;

        LevelManager.Instance.IncreaseThrow();

        this.ChangeState("Pre Throw");

        this.aimer.transform.LookAt(LevelManager.Instance.CurrentSessionInfo.throwTarget.transform.position);
    }

    private void MoveDiscHorizontally(float val){
        val = val * 2 - 1;
        var posX = val * this.discMoveDist;
        DiscSelector.Instance.SelectedDisc.transform.localPosition = DiscSelector.Instance.SelectedDisc.transform.localPosition.Set(x: posX);
        CameraFollow.Instance.transform.localPosition = CameraFollow.Instance.transform.localPosition.Set(x: posX);
    }

    public void NextHole(){
        var levelManager = LevelManager.Instance;
        levelManager.CurrentSessionInfo.throwTarget.gameObject.SetActive(false);
        levelManager.NextSession();

        var startPos = levelManager.CurrentSessionInfo.startInfo.position;
        Thrower.transform.position = startPos;
        Aimer.transform.position = startPos;
        Aimer.transform.rotation = levelManager.CurrentSessionInfo.startInfo.rotation;

        this.ChangeState("Pre Throw");
    }
    
    private void OnDestroy() {
        this.uiManager.UIAfterThrow.RemoveCallbacks();
        UIManager.Instance.UIMoveDisc.UnregisterValueChangedCallback(this.MoveDiscHorizontally);
    }


    public void SetThrowerDrive() => this.currentThrower = this.driver;
    public void SetThrowerPutt() => this.currentThrower = this.putter;

    public DiscThrower Thrower => this.currentThrower;
    public DiscAimer Aimer => this.aimer;

}
