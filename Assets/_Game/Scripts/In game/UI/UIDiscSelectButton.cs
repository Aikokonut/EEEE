using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDiscSelectButton : UIComponent
{
    [SerializeField] private Button btn;
    [SerializeField] private Image avatar;

    private int discID;
    private UIDiscSelector owner;

    private void Awake(){
        this.btn.onClick.AddListener(ButtonClickCallback);
    }

    public void Init(UIDiscSelector owner, int discID){
        this.discID = discID;
        this.owner = owner;
        this.avatar.sprite = DataResourceMapper.Instance.GetDiscResource(discID);
    }

    private void ButtonClickCallback(){
        this.owner.HidePanel();
        this.owner.TriggerOnDiscSelected(this.discID);
    }
}
