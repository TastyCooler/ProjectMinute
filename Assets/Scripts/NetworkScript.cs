using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkScript : Singleton<NetworkScript>
{

    public string HighscoreURL
    {
        get
        {
            return url;
        }
    }

    public string[] HighscoreList
    {
        get
        {
            return highscores;
        }
    }

    public static System.Action<HighscoreData> OnHighscoreDateReceived;

    [SerializeField] string url;

    string[] highscores = new string[10];

    [ContextMenu("Test")]
    // Use this for initialization
    void Start()
    {
        StartCoroutine(GetScores());
    }

    public void UpdateHighscoreList()
    {
        StartCoroutine(GetScores());
    }

    IEnumerator GetScores()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            //successful

            //parse json text
            var data = JsonUtility.FromJson<HighscoreData>(www.downloadHandler.text);
            if(data != null)
            {
                for (int i = 0; i < data.entries.Count; i++)
                {
                    if (i < HighscoreList.Length)
                    {
                        highscores[i] = data.entries[i].username + ": " + data.entries[i].score;
                    }
                }
                if (OnHighscoreDateReceived != null)
                {
                    OnHighscoreDateReceived(data);
                }
            }
        }
    }
}