using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectionManager : Semaphore
{
    public static ConnectionManager instance;
    private void Awake()
    {
        instance = this;
    }

    [HideInInspector] public string connRoute = "https://www.google.com/";
    [HideInInspector] public string dateRoute = "http://localhost/UnityBackendTutorial/GetDate.php";
    [HideInInspector] public string getUserByID = "http://localhost/UnityBackendTutorial/GetUserByID.php";

    [HideInInspector] public string scoresRoute = "http://localhost/UnityBackendTutorial/GetScores.php";
    [HideInInspector] public string addScoreRoute = "http://localhost/UnityBackendTutorial/AddScore.php";

    [HideInInspector] public string loginRoute = "http://localhost/UnityBackendTutorial/LogIn.php";
    [HideInInspector] public string signupRoute = "http://localhost/UnityBackendTutorial/SignUp.php";
    [HideInInspector] public string deleteUserRoute = "http://localhost/UnityBackendTutorial/DeleteUser.php";

    public event Action<Manager> onInitialized;
    public bool initialized;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        //if (GlobalVars.player == null)
        //{
        //  GlobalVars.SetEmptyPlayer();
        //}

        //GlobalVars.PlayerUpdated();
        //onInitialized?.Invoke(this);
        initialized = true;

        //StartCoroutine(UpdateScore((res) =>
        //{
        //    Debug.Log(res);

        //}));
    }

    //Pinging to google
    public IEnumerator CheckConnection(Action<bool> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(connRoute))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                callback(false);
            }
            else
            {
                callback(true);
            }
        }
    }

    //For sign up and log in
    public IEnumerator SubmitForm(string username, string password, string route, Action<string[]> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("submitted_username", username);
        form.AddField("submitted_password", password);

        using (UnityWebRequest request = UnityWebRequest.Post(route, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                callback(null);
            }
            else
            {
                string[] vs = request.downloadHandler.text.Split("|");
                callback(vs);
            }
        }
    }

    //For deleting account
    public IEnumerator DeleteAccount(int id, Action<string[]> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("submitted_id", id);

        using (UnityWebRequest request = UnityWebRequest.Post(deleteUserRoute, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                callback(null);
            }
            else
            {
                string[] vs = request.downloadHandler.text.Split("|");
                callback(vs);
            }
        }
    }

    //Getting scores
    public IEnumerator PerformRequest(string link, Action<string> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(link))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                callback(www.error);
            }
            else
            {
                callback(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator UpdateScore(Action<string[]> callback)
    {
        WWWForm form = new WWWForm();
        //form.AddField("submitted_id", GlobalVars.player.id);
        //form.AddField("submitted_score", GlobalVars.player.hiscore);

        using (UnityWebRequest request = UnityWebRequest.Post(addScoreRoute, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                callback(null);
            }
            else
            {
                string[] vs = request.downloadHandler.text.Split("|");
                callback(vs);
            }
        }
    }
}
