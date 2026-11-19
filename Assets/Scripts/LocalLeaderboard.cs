using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class LocalLeaderboard : MonoBehaviour
{
    public TMP_Text leaderboardText;

    private const string SaveKey = "EscapeRoom.NamedScores";
    private const int MaxScores = 5;

    [Serializable]
    public class Entry
    {
        public string initials;
        public int score;
    }

    [Serializable]
    private class SaveData
    {
        public List<Entry> entries = new List<Entry>();
    }

    private SaveData Load()
    {
        string json = PlayerPrefs.GetString(SaveKey, "");

        if (string.IsNullOrEmpty(json))
            return new SaveData();

        SaveData data = JsonUtility.FromJson<SaveData>(json);

        if (data == null || data.entries == null)
            return new SaveData();

        return data;
    }

    private void Start()
    {
        RefreshDisplay();
    }

    public void AddScore(string initials, int score)
    {
        SaveData data = Load();

        data.entries.Add(new Entry
        {
            initials = initials,
            score = score
        });

        data.entries.Sort((a, b) => b.score.CompareTo(a.score));

        if (data.entries.Count > MaxScores)
            data.entries.RemoveRange(
                MaxScores, data.entries.Count - MaxScores);

        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(data));
        PlayerPrefs.Save();

        RefreshDisplay();
    }

    public void RefreshDisplay()
    {
        if (leaderboardText == null)
            return;

        SaveData data = Load();
        StringBuilder text = new StringBuilder("TOP 5 ESCAPES\n\n");

        if (data.entries.Count == 0)
            text.Append("No scores yet!");

        for (int i = 0; i < data.entries.Count; i++)
        {
            Entry entry = data.entries[i];
            text.AppendLine(
                $"{i + 1}. {entry.initials} - {entry.score} points");
        }

        leaderboardText.text = text.ToString();
    }

    [ContextMenu("Clear Leaderboard")]
    public void ClearLeaderboard()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();
        RefreshDisplay();
    }
}