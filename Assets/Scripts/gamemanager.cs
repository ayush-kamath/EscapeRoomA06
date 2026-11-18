
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DoorController exitDoor;
    public Renderer scheduleRenderer;

    private static readonly int GlowColorId = Shader.PropertyToID("_GlowColor");

    // escape time in seconds, editable in the inspector
    public float timerDuration = 600f;
    public float RemainingSeconds { get; private set; }
    public bool TimeUp => timerExpired;

    private bool[] puzzlesCompleted = new bool[4];
    private bool exitUnlocked = false;
    private bool timerExpired = false;

    private void Awake()
    {
        RemainingSeconds = timerDuration;
    }

    private void Update()
    {
        // clock stops once the door is open or time ran out
        if (timerExpired || exitUnlocked)
            return;

        RemainingSeconds = Mathf.Max(0f, RemainingSeconds - Time.deltaTime);
        if (RemainingSeconds > 0f)
            return;

        timerExpired = true;
        Debug.Log("Time's up!");
    }

    public void CompletePuzzle(int puzzleIndex)
    {
        if (puzzleIndex < 0 || puzzleIndex >= puzzlesCompleted.Length)
            return;

        // no progress after the timer ends
        if (timerExpired)
            return;

        puzzlesCompleted[puzzleIndex] = true;

        if (puzzleIndex == 2 && scheduleRenderer != null)
        {
            var properties = new MaterialPropertyBlock();
            scheduleRenderer.GetPropertyBlock(properties);
            properties.SetColor(GlowColorId, Color.black);
            scheduleRenderer.SetPropertyBlock(properties);
        }

        if (!exitUnlocked &&
            puzzlesCompleted[0] &&
            puzzlesCompleted[1] &&
            puzzlesCompleted[2] &&
            puzzlesCompleted[3])
        {
            exitUnlocked = true;
            if (exitDoor != null)
                exitDoor.UnlockAndOpen();
            Debug.Log("All puzzles complete! Exit unlocked.");
        }
    }
}
