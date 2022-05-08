using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{

    private Animator animator;
    private Vector3 standardScale;

    // Start is called before the first frame update
    void Start()
    {
        // Gets the animator for this object
        animator = GetComponent<Animator>();

        // Sets the standard scale for this object
        standardScale = transform.localScale;
    }

    // Update is called once per frame
    void Update() {}

    // Updates the players sprite facing direktion and sets animation speed
    public void UpdatePlayerAnimation(Vector2 direction) {

        animator.SetFloat("Speed", direction.normalized.magnitude);

        if (direction.x > 0) {

            transform.localScale = standardScale; // Faces right

        } else if (direction.x < 0) {

            // Multiply the player's x local scale by -1.
            Vector3 theScale = standardScale;
            theScale.x *= -1;
            transform.localScale = theScale; // Faces left

        }
    }

    // Sets the animation to show the fox enter the portal
    public void EnterPortal() {
        animator.SetTrigger("EnterPortal");
    }
}
