using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LocalLeaderboard : MonoBehaviour
{
    public TMP_Text leaderboardText;

    private const string SaveKey = "EscapeRoom.NamedScores";
    private const int MaxScores = 5;
    private string resultHeading = "";
    private GameObject restartButton;

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
        resultHeading = won ? "" : "TIME'S UP!\n";
        if (leaderboardText != null)
        {
            var rect = leaderboardText.rectTransform;
            rect.sizeDelta = new Vector2(2.6f, 2f);
            leaderboardText.margin = Vector4.zero;
            leaderboardText.textWrappingMode = TextWrappingModes.NoWrap;
            leaderboardText.enableAutoSizing = true;
            leaderboardText.fontSizeMin = 0.25f;
            leaderboardText.fontSizeMax = 1.35f;
            leaderboardText.fontSize = 1.35f;
            leaderboardText.alignment = TextAlignmentOptions.Center;
            leaderboardText.color = Color.white;
            leaderboardText.gameObject.SetActive(true);
            PlaceInFront(rect, -0.23f, 0.1f);
        }
        RefreshDisplay();
        if (initials != null)
        {
            foreach (TMP_Text label in initials.GetComponentsInChildren<TMP_Text>(true))
                label.fontSize *= 0.7f;
            initials.gameObject.SetActive(false);
            if (enterName)
            {
                PlaceInFront(initials.transform, 0.23f, -0.08f);
                initials.BeginEntry(score);
            }
            else if (won)
            {
                TMP_Text scoreText = Instantiate(initials.promptText);
                scoreText.text = $"Score: {score}";
                scoreText.alignment = TextAlignmentOptions.Center;
                scoreText.textWrappingMode = TextWrappingModes.NoWrap;
                PlaceInFront(scoreText.transform, 0.23f, 0.1f);
            }

            Transform submitButton = initials.transform.Find("Submit Button");
            if (submitButton != null)
            {
                restartButton = Instantiate(submitButton.gameObject);
                restartButton.name = "Restart Button";
                restartButton.GetComponentInChildren<TMP_Text>().text = "RESTART";
                PlaceInFront(restartButton.transform, -0.23f, -0.3f);
                restartButton.GetComponent<XRSimpleInteractable>().selectEntered.AddListener(
                    _ => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
                restartButton.SetActive(!enterName);
            }
        }
    }

    public void ShowRestartButton()
    {
        if (restartButton != null)
            restartButton.SetActive(true);
    }

    // Keep the result controls in view as the XR camera moves.
    private static void PlaceInFront(Transform panel, float horizontalOffset, float verticalOffset = 0f)
    {
        Camera view = Camera.main;
        if (view == null) return;
        panel.SetParent(view.transform, false);
        panel.localPosition = new Vector3(horizontalOffset, verticalOffset, 0.8f);
        panel.localRotation = Quaternion.identity;
        panel.localScale = Vector3.one * 0.3f;
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
        StringBuilder text = new StringBuilder(resultHeading + "TOP 5 ESCAPES");

        if (data.entries.Count == 0)
            text.Append("\nNo scores yet!");

        for (int i = 0; i < data.entries.Count; i++)
        {
            Entry entry = data.entries[i];
            text.Append($"\n{i + 1}. {entry.initials}  {entry.score}");
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
