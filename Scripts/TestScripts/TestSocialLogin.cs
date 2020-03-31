using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.Networking;
using System;

public class TestSocialLogin : MonoBehaviour
{
    public UIButton btnGpgs;
    //public Button btnFb;
    //public Button btnSelectContentEventLogging;
    public UILabel lbGpgsSignInResult;
    //public Text txtFbSignInResult;

    public UILabel txtGpgs;//GPGS 로그인정보
    //public Text txtFb; //Facebook 로그인정보

    //public Image imgGpgs; //Gpgs 로그인정보(Google Play Game 썸네일아이콘)
    //public Image imgFb; //Facebook 로그인정보

    public System.Action<bool> OnCompleteGpgsSignIn;
    //public System.Action<bool> OnCompleteFbSignIn;

    
    void Start()
    {
        GPGSManager.instance.Init();
        //FBManager.instanace.Init();
        DontDestroyOnLoad(this);
        GpgsSignIn();
        //FaceBookSignIn();
    }

    private void GpgsSignIn()
    {
        OnCompleteGpgsSignIn = (success) =>
        {
            this.lbGpgsSignInResult.text = success.ToString();
            Debug.Log($"GpgsSignIn---------success: {success}");
            if (success)
            {
                var id = Social.localUser.id; //아이디
                var userName = Social.localUser.userName; //유저이름
                var underAge = Social.localUser.underage; //연령               

                //StartCoroutine(FireBaseAuthorization());


                //StartCoroutine(this.WaitForLoadImage(() =>
                //{
                //    var width = this.imgGpgs.rectTransform.rect.width;
                //    var height = this.imgGpgs.rectTransform.rect.height;
                //    var image = Social.localUser.image;
                //    this.imgGpgs.sprite = Sprite.Create(image, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
                //    this.txtGpgs.text = string.Format("id: {0}\nuserName: {1}\n", id, userName);
                //}));

            }
        };

        this.btnGpgs.onClick.Add(new EventDelegate(() =>
        {
            Debug.Log("SignIn Button Pressed");
            GPGSManager.instance.SignIn(OnCompleteGpgsSignIn);
        }));
    }
    //IEnumerator FireBaseAuthorization()
    //{
    //    Debug.Log($"-----------FireBaseAuthorization-----------");
    //    string idToken = string.Empty;
    //    while (true)
    //    {
    //        var playGamesLocalUser = (PlayGamesLocalUser)Social.localUser;
    //        idToken = playGamesLocalUser.GetIdToken();
    //        if (!string.IsNullOrEmpty(idToken))
    //            break;
    //        yield return null;
    //    }
    //    Debug.Log($"--------------------> token: {idToken}");
    //    var auth = FirebaseAuth.DefaultInstance;
    //    Debug.Log($"auth: {auth}");
    //    Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

    //    auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
    //    {
    //        Debug.Log($"task.Status: {task.Status}");

    //        if (task.IsCanceled)
    //        {
    //            Debug.LogError("SignInWithCredentialAsync was canceled.");
    //            return;
    //        }
    //        if (task.IsFaulted)
    //        {
    //            Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
    //            return;
    //        }

    //        Firebase.Auth.FirebaseUser newUser = task.Result;
    //        Debug.LogFormat("User signed in successfully: {0} ({1})",
    //            newUser.DisplayName, newUser.UserId);
    //    });
    //}

    IEnumerator WaitForLoadImage(System.Action OnLoadImage)
    {
        Debug.Log("StartCoroutine");

        yield return ((PlayGamesLocalUser)Social.localUser).LoadImage();
        Debug.Log("yield return LoadImage");

        while (true)
        {
            if (Social.localUser.image != null)
            {
                Debug.Log($"image Loaded");
                break;
            }
            yield return null;
        }

        OnLoadImage();
    }
}
