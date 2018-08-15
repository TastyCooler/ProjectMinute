using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighscoreData
{

    [System.Serializable]
    public class HighscoreDataEntry
    {
        public HighscoreDataEntry()
        {
            username = "";
            score = 0;
        }

        public string username;
        public int score;
    }

    public List<HighscoreDataEntry> entries;

    public HighscoreData()
    {
        entries = new List<HighscoreDataEntry>();
    }

}
