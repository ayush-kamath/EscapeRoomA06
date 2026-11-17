
using UnityEngine;

public class Puzzle1Keycard : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject exitCard;
    public GameObject bomb;

    private bool completed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (completed)
        {
            return;
        }

        if (other.attachedRigidbody == null)
        {
            return;
        }

        if (other.attachedRigidbody.gameObject != exitCard)
        {
            return;
        }

        completed = true;

        gameManager.CompletePuzzle(0);

        if (bomb != null)
        {
            bomb.SetActive(true);
        }

        Debug.Log("Puzzle 1 completed! Bomb activated!");
    }
}
