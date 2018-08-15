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
            user = "";
            score = 0;
        }

        public string user;
        public int score;
    }

    public List<HighscoreDataEntry> entries;

    public HighscoreData()
    {
        entries = new List<HighscoreDataEntry>();
    }

}
