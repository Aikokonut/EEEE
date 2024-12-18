using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AuthenticationManager : MonoBehaviour
{
    public static AuthenticationManager Instance;

    [SerializeField] private UnityEvent<UserInfo> OnSignInSuccess;
    [SerializeField] private UnityEvent OnSignInFailed;

    private IAuthenticator googleLogin;

    private void Awake(){
        Instance = this;
        googleLogin = new FirebaseAuthenticator();
    }

    public void SignInWithGoogle(){
        googleLogin.SignIn(this.OnSignInSuccess, this.OnSignInFailed);
    }
}
