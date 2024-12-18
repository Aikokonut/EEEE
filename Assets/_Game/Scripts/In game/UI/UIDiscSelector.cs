using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIDiscSelector : UIComponent
{
    [SerializeField] private Transform container;
    [SerializeField] private Button openBtn;
    [SerializeField] private UIDiscSelectButton btnPrefab;
    [SerializeField] private GameObject panel;

    private UnityAction<int> OnDiscSelected;

    private void Awake(){
        //this.openBtn.onClick.AddListener(this.Show);
        this.panel.SetActive(false);
        //this.gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        this.panel.SetActive(true);

        var removeList = new List<GameObject>();
        for (int i = 0; i < container.childCount; i++)
        {
            removeList.Add(container.GetChild(i).gameObject);
        }
        removeList.ForEach(x => Destroy(x));

        var ownedDiscIDList = DataManager.Instance.GetDiscData();
        foreach (var id in ownedDiscIDList)
        {
            var btn = Instantiate(btnPrefab);
            btn.transform.SetParent(this.container);
            btn.Init(this, id);
        }
    }

    public void ShowUI(){
        this.gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        this.panel.SetActive(false);
    }

    public void HideUI() => this.gameObject.SetActive(false);

    public void RegisterDiscSelectCallback(UnityAction<int> callback)
    {
        this.OnDiscSelected += callback;
    }
    public void UnregisterDiscSelectCallback(UnityAction<int> callback)
    {
        this.OnDiscSelected -= callback;
    }
    public void TriggerOnDiscSelected(int id)
    {
        OnDiscSelected?.Invoke(id);
    }
}
