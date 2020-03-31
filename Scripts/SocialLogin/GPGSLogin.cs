using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.Networking;
using Firebase.Auth;
using System;


public class GPGSLogin : MonoBehaviour
{
    public System.Action<bool> OnCompleteGpgsSignIn;
    public FirebaseUser firbaseUser;
    public void CheckVersionOfGPGS()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void GpgsSignIn()
    {
        OnCompleteGpgsSignIn = (success) =>
        {
            Debug.Log($"GpgsSignIn---------success: {success}");
            if (success)
            {
                var id = Social.localUser.id; //아이디
                var userName = Social.localUser.userName; //유저이름
                var underAge = Social.localUser.underage; //연령               
                Debug.Log($"GPGSLogin_GpgsSignIn id:{id}");
                StartCoroutine(FireBaseAuthorization());
            }
        };
        GPGSManager.instance.SignIn(OnCompleteGpgsSignIn);
    }
    IEnumerator FireBaseAuthorization()
    {
        Debug.Log($"-----------FireBaseAuthorization-----------");
        string idToken = string.Empty;
        while (true)
        {
            var playGamesLocalUser = (PlayGamesLocalUser)Social.localUser;
            idToken = playGamesLocalUser.GetIdToken();
            if (!string.IsNullOrEmpty(idToken))
                break;
            yield return null;
        }
        Debug.Log($"--------------------> token: {idToken}");
        var auth = FirebaseAuth.DefaultInstance;
        Debug.Log($"auth: {auth}");
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            Debug.Log($"task.Status: {task.Status}");

            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            this.firbaseUser = newUser;
        });
    }
}

