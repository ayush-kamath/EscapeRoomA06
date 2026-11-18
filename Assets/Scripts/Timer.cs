
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    public GameManager gameManager;

    void Update()
    {
        float timeRemaining = gameManager.RemainingSeconds;

        if (gameManager.TimeUp)
        {
            timerText.text = "YOU LOSE!";
            timerText.color = Color.red;
            return;
        }

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        if (gameManager.HasWon)
        {
            timerText.text = string.Format(
                "YOU ESCAPED!\nTime left: {0:00}:{1:00}\nScore: {2}",
                minutes, seconds, gameManager.FinalScore
            );
            timerText.color = Color.green;
            return;
        }

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
