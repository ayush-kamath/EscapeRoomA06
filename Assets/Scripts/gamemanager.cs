
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DoorController exitDoor;

    private const float TimerDuration = 30f;
    public float RemainingSeconds { get; private set; } = TimerDuration;

    private bool[] puzzlesCompleted = new bool[3];
    private bool exitUnlocked = false;
    private bool timerExpired = false;

    private void Update()
    {
        if (timerExpired)
            return;

        RemainingSeconds = Mathf.Max(0f, RemainingSeconds - Time.deltaTime);
        if (RemainingSeconds > 0f)
            return;

        timerExpired = true;
        Debug.Log("Time's up!");
    }

    public void CompletePuzzle(int puzzleIndex)
    {
        if (puzzleIndex < 0 || puzzleIndex >= 3)
            return;

        puzzlesCompleted[puzzleIndex] = true;

        if (!exitUnlocked &&
            puzzlesCompleted[0] &&
            puzzlesCompleted[1] &&
            puzzlesCompleted[2])
        {
            exitUnlocked = true;
            exitDoor.UnlockAndOpen();
            Debug.Log("All puzzles complete! Exit unlocked.");
        }
    }
}
