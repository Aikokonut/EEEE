using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelComplete : UIComponent
{
    [SerializeField] private Button mainMenuBtn;

    private void Awake(){
        this.mainMenuBtn.onClick.AddListener(this.MainMenuBtnClick);
    }

    private void MainMenuBtnClick(){
        var gameManager = Singleton<GameManager>.Instance;
        gameManager.LoadMainMenu();
    }
}
