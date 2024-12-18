using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private List<LevelSession> sessionList;
    [SerializeField] private LevelSession currentSessionInfo;
    private int currentSession;
    private int currentThrow;
    private UnityAction OnLevelComplete;

    public LevelSession CurrentSessionInfo => this.currentSessionInfo;
    public bool IsLastSession => this.currentSession == this.sessionList.Count - 1;
    public int CurrentThrow => this.currentThrow;
    public int CurrentSession => this.currentSession;

    private void Awake(){
        Instance = this;
        UIManager.Instance.UIDiscSelector.ShowUI();
        UIManager.Instance.UIThrowSelector.ShowUI();
        UIManager.Instance.UIMiniMap.ShowUI();  
    }

    private void Start() {
        this.StartSession(0);
    }

    public void StartSession(int index){
        this.currentSession = index;
        this.currentSessionInfo = sessionList[index];
        this.currentThrow = 0;
        DL.Utils.CoroutineUtils.Invoke(this, this.currentSessionInfo.sessionBound.InitializeSessionMap, 0);
    }

    public void RegisterLevelCompleteCallback(UnityAction callback){
        this.OnLevelComplete += callback;
    }
    public void UnregisterLevelCompleteCallback(UnityAction callback){
        this.OnLevelComplete += callback;
    }

    public void NextSession(){
        if(currentSession == sessionList.Count - 1){
            this.LevelComplete();
            return;
        }
        this.currentSessionInfo.throwTarget.gameObject.SetActive(false);
        StartSession(currentSession + 1);
        this.currentSessionInfo.throwTarget.gameObject.SetActive(true);
    }

    private void LevelComplete(){
        UIManager.Instance.UILevelComplete.ShowUI();
        this.OnLevelComplete?.Invoke();
    }

    public void IncreaseThrow() => this.currentThrow++;
}   
[System.Serializable]
public class LevelSession{
    public ThrowTarget throwTarget;
    public Transform startInfo;
    public SessionBound sessionBound;
    public int par;
}
