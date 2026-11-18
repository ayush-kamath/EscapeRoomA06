
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DoorController exitDoor;
    [SerializeField] private Renderer scheduleRenderer;

    private static readonly int GlowColorId = Shader.PropertyToID("_GlowColor");

    private const float TimerDuration = 30f;
    public float RemainingSeconds { get; private set; } = TimerDuration;

    private bool[] puzzlesCompleted = new bool[4];
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
        if (puzzleIndex < 0 || puzzleIndex >= puzzlesCompleted.Length)
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
            exitDoor.UnlockAndOpen();
            Debug.Log("All puzzles complete! Exit unlocked.");
        }
    }
}
