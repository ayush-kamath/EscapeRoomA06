
using UnityEngine;

public class Puzzle1Keycard : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject exitCard;
    public GameObject puzzle2;

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

        if (exitCard.TryGetComponent<Renderer>(out var cardRenderer))
        {
            var properties = new MaterialPropertyBlock();
            cardRenderer.GetPropertyBlock(properties);
            properties.SetColor("_GlowColor", Color.black);
            cardRenderer.SetPropertyBlock(properties);
        }

        if (gameManager != null)
        {
            gameManager.CompletePuzzle(0);
        }

        if (puzzle2 != null)
        {
            puzzle2.SetActive(true);
            Debug.Log("Puzzle 2 activated!");
        }

        Debug.Log("Puzzle 1 completed!");
    }
}
