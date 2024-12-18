using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private void Awake(){
        UIManager.Instance.UILevelComplete.HideUI();
        UIManager.Instance.UIPreThrow.HideUI();
        UIManager.Instance.UILevelComplete.HideUI();
        UIManager.Instance.UILevelComplete.HideUI();
        UIManager.Instance.UIAfterThrow.HideUI();
        UIManager.Instance.UIMiniMap.HideUI();

        UIManager.Instance.UIMainMenu.ShowUI();
    }
}
