using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthCallback : MonoBehaviour
{
    [SerializeField] private GameObject testobj;
    public void HandleLoginSuccess(UserInfo user){
        Debug.Log(user.displayName);
        testobj.gameObject.SetActive(true);
    }
    
}
