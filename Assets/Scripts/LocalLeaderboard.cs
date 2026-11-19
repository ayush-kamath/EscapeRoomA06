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
    private string resultHeading = "";

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

        SaveData data;
        try { data = JsonUtility.FromJson<SaveData>(json); }
        catch (ArgumentException) { return new SaveData(); }

        if (data == null || data.entries == null)
            return new SaveData();

        data.entries.RemoveAll(entry => entry == null || entry.score < 0);
        data.entries.Sort((a, b) => b.score.CompareTo(a.score));
        if (data.entries.Count > MaxScores)
            data.entries.RemoveRange(MaxScores, data.entries.Count - MaxScores);
        return data;
    }

    private void Awake()
    {
        if (leaderboardText != null) leaderboardText.gameObject.SetActive(false);
    }

    public bool Qualifies(int score)
    {
        var entries = Load().entries;
        return score > 0 && (entries.Count < MaxScores || score > entries[entries.Count - 1].score);
    }

    public void ShowResults(int score, bool won, InitialsEntry initials)
    {
        bool enterName = won && Qualifies(score) && initials != null;
        resultHeading = won ? $"YOU ESCAPED!  Score: {score}\n\n" : "TIME'S UP!\n\n";
        if (leaderboardText != null)
        {
            var rect = leaderboardText.rectTransform;
            rect.sizeDelta = new Vector2(2.4f, 2.4f);
            leaderboardText.margin = Vector4.zero;
            leaderboardText.enableAutoSizing = false;
            leaderboardText.fontSize = 2f;
            leaderboardText.alignment = TextAlignmentOptions.TopLeft;
            leaderboardText.color = Color.white;
            leaderboardText.gameObject.SetActive(true);
            PlaceInFront(rect, enterName ? -0.8f : 0f);
        }
        RefreshDisplay();
        if (initials != null)
        {
            initials.gameObject.SetActive(false);
            if (enterName)
            {
                PlaceInFront(initials.transform, 0.8f);
                initials.BeginEntry(score);
            }
        }
    }

    // Place once at eye height, then leave the controls stationary for XR pointing.
    private static void PlaceInFront(Transform panel, float horizontalOffset)
    {
        Camera view = Camera.main;
        if (view == null) return;
        Vector3 forward = Vector3.ProjectOnPlane(view.transform.forward, Vector3.up).normalized;
        if (forward.sqrMagnitude < 0.01f) forward = Vector3.forward;
        Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
        Vector3 origin = view.transform.position;
        origin.y = Mathf.Max(origin.y, 0.35f);
        float panelScale = origin.y < 0.9f ? 0.25f : 0.65f;
        float distance = 2f;
        if (Physics.SphereCast(origin, 0.25f, forward, out RaycastHit hit, distance,
            Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            distance = Mathf.Clamp(hit.distance - 0.15f, 0.4f, 2f);
        float distanceScale = distance / 2f;
        panel.SetParent(null, true);
        Vector3 position = origin + forward * distance
            + rotation * Vector3.right * horizontalOffset * distanceScale * (panelScale / 0.65f);
        // The simulator can start with its head at ground level.
        position.y = Mathf.Max(position.y, 0.35f);
        panel.SetPositionAndRotation(position, rotation);
        panel.localScale = Vector3.one * panelScale * distanceScale;
    }

    public void AddScore(string initials, int score)
    {
        if (!Qualifies(score)) return;
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
        StringBuilder text = new StringBuilder(resultHeading + "TOP 5 ESCAPES\n\n");

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
