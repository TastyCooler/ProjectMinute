using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkScript : MonoBehaviour
{

    [SerializeField] string url;

    [ContextMenu("Test")]
    // Use this for initialization
    void Start()
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
            Debug.Log(www.downloadHandler.text);

            //parse json text
            var data = JsonUtility.FromJson<HighscoreData>(www.downloadHandler.text);
            for (int i = 0; i < data.entries.Count; i++)
            {
                Debug.Log(data.entries[i].user + ": " + data.entries[i].score);
            }
        }
    }
}