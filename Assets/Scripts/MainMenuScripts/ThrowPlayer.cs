using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPlayer : MonoBehaviour {

    private Rigidbody2D ridgidBody;
    private bool moveAllowed = false;
    private bool mouseMoveAllowed = false;
    [SerializeField] private float throwBonus = 20;
    [SerializeField] private float throwAirDrag = 20;
    [SerializeField] private bool enableRagdollRotation = true;
    [SerializeField] private PlayerMovement disableMovment;
    [SerializeField] private CharacterController2D disableCharacterController;
    [SerializeField] private Animator animator;


    // Setup the original position
    void Start() {

        ridgidBody = GetComponent<Rigidbody2D>(); // Gets ridgid body connected to this gameobject

    }

    // Update is called once per frame
    void Update() {

        // If there were any touches on the screen
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            // Get the position of the touch 
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);


            switch (touch.phase) {
                // At the moment the screen was touched, look if the touch is on the object.
                // If the touch was on the object, set moveAllowed to true
                case TouchPhase.Began:
                    if (GetComponent<BoxCollider2D>() == Physics2D.OverlapPoint(touchPos) || GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(touchPos)) {
                        moveAllowed = true;
                        GetComponent<CircleCollider2D>().sharedMaterial = null;

                        setUpRidgidbody();
                    }
                    break;

                // While the touch is moving and is allowed to move, update the object position
                // to the touchs position
                case TouchPhase.Moved:
                    if (moveAllowed) {
                        addForceToRigidbody(touchPos);
                    }
                    break;

                // While the touch is stationary and is allowed to move, update the object position
                // to the touchs position
                case TouchPhase.Stationary:
                    if (moveAllowed) {
                        addForceToRigidbody(touchPos);
                    }
                    break;

                // If the touch ended(player let go off the screen), set moveAllowed to false
                case TouchPhase.Ended:
                    
                    // Reset
                    if (moveAllowed) {
                        moveAllowed = false;
                        resetRidgidbody();
                    }

                    break;

            }
        }

        // For mouse controls
        else {
            // Get the mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // If the mouse is pressed down and the mouse is over the object, set mouseMoveAllowed to true
            if (Input.GetMouseButtonDown(0) && (GetComponent<BoxCollider2D>() == Physics2D.OverlapPoint(mousePosition) || GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(mousePosition))) {
                mouseMoveAllowed = true;
                setUpRidgidbody();
            }

            // If mouseMoveAllowed is true, set the object to follow the mouse until the mouse button is released
            if (mouseMoveAllowed) {

                addForceToRigidbody(mousePosition);

                // Reset gravity and drag when mouse is not pressed
                if (Input.GetMouseButtonUp(0)) {
                    resetRidgidbody();
                    mouseMoveAllowed = false;
                }
            }
        }
        updateOtherMovmentScripts();
     } 

    // Sets the ridgidbody up for beeing draged
    private void setUpRidgidbody() {
        ridgidBody.gravityScale = 0;
        ridgidBody.drag = throwAirDrag;
        if (enableRagdollRotation) { ridgidBody.freezeRotation = false; }
    }

    // Resets the ridgidbody to it's initial state
    private void resetRidgidbody() {
        ridgidBody.gravityScale = 3;
        ridgidBody.drag = 0;
    }


    // Adds force to the player ridgidbody to move towards finger/cursor
    private void addForceToRigidbody(Vector3 toPosition) {
        Vector2 forceVector = (toPosition - transform.position) * throwBonus;
        ridgidBody.AddForce(forceVector);
    }

    // Enables / disables other scripts based on if this is currently active and doing stuff
    private void updateOtherMovmentScripts() {
        
        if (mouseMoveAllowed || moveAllowed) {
            disableMovment.enabled = false;
            disableCharacterController.enabled = false;
            animator.SetBool("IsDraging", true);
        } else if (!mouseMoveAllowed && !moveAllowed && ridgidBody.velocity == Vector2.zero) {
            disableMovment.enabled = true;
            disableCharacterController.enabled = true;
            animator.SetBool("IsDraging", false);

            if (enableRagdollRotation) {
                transform.rotation = Quaternion.Euler(Vector3.zero);
                ridgidBody.freezeRotation = true;
            }
        }
      
    }

}