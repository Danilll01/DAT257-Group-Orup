using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RandomBirdFlyingScript : MonoBehaviour
{
    [SerializeField] private float flyingRadius = 1;
    private Vector3 startingLocation;
    private Vector3 targetPosition;
    private float timer = 0f;
    private Rigidbody2D ridgidBody;

    // Start is called before the first frame update
    void Start()
    {
        startingLocation = transform.position;
        ridgidBody = GetComponent<Rigidbody2D>();
        targetPosition = getRandomPoint();
        timer = Random.Range(0.5f, 2);
    }

    // Update is called once per frame
    void Update()
    {

        timer -= Time.deltaTime;

        if (timer < 0) {
            targetPosition = getRandomPoint();
            timer = Random.Range(0.5f, 2);
        }

        addForceToRidgidbody();
        
    }

    private Vector2 getRandomPoint() {
        Vector3 randomDirection = Random.insideUnitSphere * flyingRadius;
        randomDirection += startingLocation;
        return randomDirection;
    }

    // Adds force to the player ridgidbody to move towards the target point
    private void addForceToRidgidbody() {
      
        Vector2 forceVector = (targetPosition - transform.position) * 15 * (Time.deltaTime * 10);
        ridgidBody.AddForce(forceVector);
      
    }
}
