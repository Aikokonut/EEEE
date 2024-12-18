using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UILevelInfo : UIComponent
{
    [SerializeField] private TMP_Text textCurrentCount, textPar, textCurrentSession, textDistance;
    private LevelManager levelManager => LevelManager.Instance;

    private void Update(){
        this.UpdateInfo();
    }

    private void UpdateInfo(){
        this.textCurrentCount.text = levelManager.CurrentThrow.ToString();
        this.textPar.text = levelManager.CurrentSessionInfo.par.ToString();
        this.textCurrentSession.text = levelManager.CurrentSession.ToString();

        var discPos = DiscSelector.Instance.SelectedDisc.transform.position;
        var targetPos = levelManager.CurrentSessionInfo.throwTarget.transform.position;
        this.textDistance.text = Mathf.RoundToInt((targetPos - discPos).magnitude).ToString() + "m";
    }
}
