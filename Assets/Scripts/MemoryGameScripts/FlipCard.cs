using UnityEngine;

public class FlipCard : MonoBehaviour
{
    [SerializeField] private float lerpTime = 0.015f;

    private Vector3 targetDegrees;
    private Vector3 currentDegrees;

    public void Start()
    {
        targetDegrees = transform.eulerAngles;
        currentDegrees = transform.eulerAngles;
    }

    public void FixedUpdate()
    {
        // If card should rotate
        if (currentDegrees != targetDegrees)
        {
            // Rotate the card to target position with Lerp
            transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, targetDegrees, lerpTime);

            // Update current angle
            currentDegrees = transform.eulerAngles;
        }
    }

    // Flips card so the animal is not showing
    public void FlipCardToOriginalState()
    {
        targetDegrees = new(0, 0, 0);
    }

    // Flips card so the animal is showing
    public void FlipCardToAnimalState()
    {
        targetDegrees = new(0, 180, 0);
    }
}
