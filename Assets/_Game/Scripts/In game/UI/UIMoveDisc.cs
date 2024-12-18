using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIMoveDisc : UIComponent
{
    private Slider slider;
    private UnityAction<float> OnSliderValueChanged;

    public void Init(){
        this.slider = GetComponent<Slider>();
        this.slider.onValueChanged.AddListener(TriggerCallback);
    }

    private void TriggerCallback(float value){
        this.OnSliderValueChanged?.Invoke(value);
    }

    public void RegisterValueChangedCallback(UnityAction<float> callback){
        this.OnSliderValueChanged += callback;
    }
    public void UnregisterValueChangedCallback(UnityAction<float> callback){
        this.OnSliderValueChanged -= callback;
    }
}
