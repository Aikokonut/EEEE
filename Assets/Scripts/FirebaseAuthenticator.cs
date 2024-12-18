using System.Collections;
using System.Collections.Generic;
using Google;
using UnityEngine;
using UnityEngine.Events;
using Firebase.Extensions;

public class FirebaseAuthenticator : IAuthenticator
{
    private GoogleSignInConfiguration configuration;
    private string clientID = "501657621421-1089dsj0cit4doagi6nks57egqg3vc1d.apps.googleusercontent.com";
    public string ClientID => this.clientID;

    public FirebaseAuthenticator()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = clientID,
            RequestIdToken = true,
        };
    }

    public void SignIn(UnityEvent<UserInfo> successCallback, UnityEvent failCallback)
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith((task) =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Faulted");
                failCallback?.Invoke();
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("Cancelled");
                failCallback?.Invoke();
            }
            else
            {
                Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
                var auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

                auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
                {
                    if (task.IsCanceled)
                    {
                        return;
                    }

                    if (task.IsFaulted)
                    {
                        Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                        return;
                    }

                    var user = auth.CurrentUser;

                    var userInfo = new UserInfo(){
                        userID = user.UserId,
                        displayName = user.DisplayName
                    };
                    successCallback?.Invoke(userInfo);
                });
            }
        });
    }
}
