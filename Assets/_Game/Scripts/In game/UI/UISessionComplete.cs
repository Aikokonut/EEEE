using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISessionComplete : UIComponent
{
    [SerializeField] private Button nextHoleBtn;

    public void RegisterNextHoleClick(UnityAction callback){
        this.nextHoleBtn.onClick.AddListener(callback);
    }   

    public void RemoveCallbacks(){
        this.nextHoleBtn.onClick.RemoveAllListeners();
    }
}
