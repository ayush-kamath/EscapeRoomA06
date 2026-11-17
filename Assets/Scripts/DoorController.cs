using UnityEngine;
using q = UnityEngine.Quaternion;

public class DoorController : MonoBehaviour
{
    public float openAngle = 90f;
    public float rotationSpeed = 90f;
    public bool testOpen = false;

    private bool isOpen = false;
    private q closedRotation;
    private q openRotation;

    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * q.Euler(0, openAngle, 0);
    }

    void Update()
    {
        q target;

        if (isOpen || testOpen)
        {
            target = openRotation;
        }
        else
        {
            target = closedRotation;
        }

        transform.localRotation = q.RotateTowards(
            transform.localRotation,
            target,
            rotationSpeed * Time.deltaTime
        );
    }

    public void UnlockAndOpen()
    {
        isOpen = true;
    }
}