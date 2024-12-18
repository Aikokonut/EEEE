using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComponent : MonoBehaviour
{
    public void ShowUI()
    {

        this.gameObject.SetActive(true);
    }
    public void HideUI()
    {
        this.gameObject.SetActive(false);
    }
}
