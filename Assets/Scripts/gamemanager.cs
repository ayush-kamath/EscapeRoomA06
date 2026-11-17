
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DoorController exitDoor;

    private bool[] puzzlesCompleted = new bool[3];
    private bool exitUnlocked = false;

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
