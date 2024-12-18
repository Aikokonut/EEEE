using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private UIManager uiManager;
    protected override void Awake()
    {
        base.Awake();
        uiManager.Init();
        this.LoadMainMenu();
    }

    public void LoadGameMap(string gameMap)
    {
        SceneManager.UnloadSceneAsync("MainMenu").completed += (op) =>
        {
            SceneManager.LoadSceneAsync(gameMap, LoadSceneMode.Additive).completed += (op) =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
                UIManager.Instance.UIMainMenu.HideUI();
            };
        };
    }

    public void LoadMainMenu()
    {
        if (SceneManager.sceneCount > 1)
        {
            var activeScene = SceneManager.GetActiveScene();
            SceneManager.UnloadSceneAsync(activeScene).completed += (op) =>
            {
                SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
            };
        }
        else
        {
            SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
        }
    }
}
