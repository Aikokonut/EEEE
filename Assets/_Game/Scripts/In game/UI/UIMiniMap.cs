using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMiniMap : UIComponent
{
    [SerializeField] private RectTransform discMarker, targetMarker;
    public Vector2 Size => this.GetComponent<RectTransform>().sizeDelta;

    public void SetDiscMarkerPosition(Vector2 pos){
        Debug.Log(pos);
        discMarker.anchoredPosition = pos;
    }
    public void SetTargetMarkerPosition(Vector2 pos){
        targetMarker.anchoredPosition = pos;
    }
    public void SetMapTexture(Texture2D tex){
        this.GetComponent<RawImage>().texture = tex;
    }
}
