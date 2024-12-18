using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IAuthenticator
{
    public void SignIn(UnityEvent<UserInfo> successCallback, UnityEvent failCallback);
}

public struct UserInfo{
    public string userID;
    public string displayName;
}


