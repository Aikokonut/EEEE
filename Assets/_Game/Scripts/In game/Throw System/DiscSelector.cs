using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public class DiscSelector : Sirenix.OdinInspector.SerializedMonoBehaviour
{
    public static DiscSelector Instance;
    [SerializeField] private DiscThrower driver, putter;
    [SerializeField] private Dictionary<int, Disc> discList;

    private DiscThrower selectedThrower;
    private Disc selectedDisc;

    public DiscThrower SelectedThrower => this.selectedThrower;
    public Disc SelectedDisc => this.selectedDisc;

    private void Start(){
        UIManager.Instance.UIDiscSelector.RegisterDiscSelectCallback(this.SetDisc);
    }

    private void Awake() {
        Instance = this;
        discList.ForEach(x => x.Value.gameObject.SetActive(false));
        this.selectedDisc = discList[0];
        this.selectedDisc.gameObject.SetActive(true);
        this.SetThrowerDrive();
    }


    public void SetThrowerDrive()
    {
        this.driver.gameObject.SetActive(true);
        this.putter.gameObject.SetActive(false);
        UIManager.Instance.UIPreThrow.UIBender.gameObject.SetActive(true);
        this.selectedThrower = this.driver;
    }
    public void SetThrowerPutt()
    {
        this.driver.gameObject.SetActive(false);
        this.putter.gameObject.SetActive(true);
        UIManager.Instance.UIPreThrow.UIBender.gameObject.SetActive(false);
        this.selectedThrower = this.putter;

    }

    public void SetDisc(int discIndex){
        this.selectedDisc.gameObject.SetActive(false);
        this.selectedDisc = this.discList[discIndex];
        this.selectedDisc.gameObject.SetActive(true);
    }

    private void OnDestroy() {
        UIManager.Instance.UIDiscSelector.UnregisterDiscSelectCallback(this.SetDisc);
    }
}
