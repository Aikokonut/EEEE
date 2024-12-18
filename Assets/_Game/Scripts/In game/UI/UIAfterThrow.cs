using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIAfterThrow : UIComponent
{
    [SerializeField] private Button throwAgainBtn, moveToTieBtn;

    public void RegisterThrowAgainClick(UnityAction callback){
        this.throwAgainBtn.onClick.AddListener(callback);
    }

    public void RegisterMoveToTieClick(UnityAction callback){
        this.moveToTieBtn.onClick.AddListener(callback);
    }

    public void RemoveCallbacks(){
        this.throwAgainBtn.onClick.RemoveAllListeners();
        this.moveToTieBtn.onClick.RemoveAllListeners();
    }
}
