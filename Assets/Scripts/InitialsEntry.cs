using TMPro;
using UnityEngine;

public class InitialsEntry : MonoBehaviour
{
    public LocalLeaderboard leaderboard;

    public TMP_Text firstLetter;
    public TMP_Text secondLetter;
    public TMP_Text thirdLetter;
    public TMP_Text promptText;

    private readonly char[] letters = { 'A', 'A', 'A' };
    private int pendingScore;
    private bool acceptingInput;

    public void BeginEntry(int score)
    {
        pendingScore = score;
        acceptingInput = true;

        letters[0] = 'A';
        letters[1] = 'A';
        letters[2] = 'A';

        gameObject.SetActive(true);
        RefreshLetters();

        promptText.text = $"Score: {score}\nEnter your initials";
    }

    private void ChangeLetter(int index, int direction)
    {
        if (!acceptingInput)
            return;

        int value = letters[index] - 'A';
        value = (value + direction + 26) % 26;
        letters[index] = (char)('A' + value);

        RefreshLetters();
    }

    private void RefreshLetters()
    {
        firstLetter.text = letters[0].ToString();
        secondLetter.text = letters[1].ToString();
        thirdLetter.text = letters[2].ToString();
    }

    public void FirstUp() => ChangeLetter(0, 1);
    public void FirstDown() => ChangeLetter(0, -1);

    public void SecondUp() => ChangeLetter(1, 1);
    public void SecondDown() => ChangeLetter(1, -1);

    public void ThirdUp() => ChangeLetter(2, 1);
    public void ThirdDown() => ChangeLetter(2, -1);

    public void SubmitScore()
    {
        if (!acceptingInput || leaderboard == null)
            return;

        // Lock input before saving to prevent duplicate submissions.
        acceptingInput = false;

        leaderboard.AddScore(new string(letters), pendingScore);
        gameObject.SetActive(false);
    }
}