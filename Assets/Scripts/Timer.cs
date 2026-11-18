
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

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
