using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIBender : UIComponent, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform center;
    [SerializeField] private float maxDragSize, minDragSize, maxAngle;
    [SerializeField] private Canvas parentCanvas;

    private float sinMaxAngle;
    private Vector2 placeholder;
    private bool isDragging;

    private UnityAction<float, bool> onKnotDrop;
    private UnityAction<float, float> onKnotDrag; //Drag length, angle length

    private RectTransform rectTransform => this.GetComponent<RectTransform>();

    private void Start()
    {
        this.sinMaxAngle = Mathf.Sin(this.maxAngle * Mathf.Deg2Rad);
        this.placeholder = rectTransform.anchoredPosition;
    }

    public void RegisterOnDragCallback(UnityAction<float, float> callback)
    {
        this.onKnotDrag += callback;
    }
    public void RegisterOnDropCallback(UnityAction<float, bool> callback)
    {
        this.onKnotDrop += callback;
    }
    public void UnregisterOnDragCallback(UnityAction<float, float> callback)
    {
        this.onKnotDrag += callback;
    }
    public void UnregisterOnDropCallback(UnityAction<float, bool> callback)
    {
        this.onKnotDrop += callback;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.isDragging = true;
        placeholder += eventData.delta * parentCanvas.scaleFactor;
        var length = Vector2.Distance(placeholder, center.anchoredPosition);
        var oldPos = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = placeholder;
        if (length > maxDragSize)
        {
            rectTransform.anchoredPosition = center.anchoredPosition + (placeholder - center.anchoredPosition).normalized * maxDragSize;
        }
        if (Mathf.Abs(placeholder.x / length) > sinMaxAngle)
            rectTransform.anchoredPosition = oldPos;
        this.onKnotDrag?.Invoke(Vector2.Distance(rectTransform.anchoredPosition, center.anchoredPosition), rectTransform.anchoredPosition.x);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = center.anchoredPosition;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            var length = Vector2.Distance(rectTransform.anchoredPosition, center.anchoredPosition);
            if (length > minDragSize)
            {
                this.onKnotDrop?.Invoke(length, true);
            }
            else this.onKnotDrop?.Invoke(length, false);
            rectTransform.anchoredPosition = center.anchoredPosition;
            placeholder = center.anchoredPosition;
            this.isDragging = false;
        }
    }
}
