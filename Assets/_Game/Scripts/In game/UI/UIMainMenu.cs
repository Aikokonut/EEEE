using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : UIComponent
{
    [SerializeField] private Button loadGameBtn;

    private void Awake()
    {
        loadGameBtn.onClick.AddListener(() =>
        {
            var loadParams = new LoadSceneParameters()
            {
                loadSceneMode = LoadSceneMode.Additive,
                localPhysicsMode = LocalPhysicsMode.Physics3D
            };
            Camera.main.gameObject.SetActive(false);
            // SceneManager.LoadSceneAsync("Game_LakeSide", LoadSceneMode.Additive).completed += (op) =>
            // {
            //     SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
            //     this.gameObject.SetActive(false);
            // };
            var gameManager = Singleton<GameManager>.Instance;
            gameManager.LoadGameMap("Game_LakeSide");
        });
    }
}
