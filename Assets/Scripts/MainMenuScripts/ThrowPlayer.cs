using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPlayer : MonoBehaviour {

    private Rigidbody2D rigidBody;
    private float deltaX, deltaY;
    private bool moveAllowed = false;
    private bool mouseMoveAllowed = false;
    [SerializeField] private float throwBonus = 20;
    [SerializeField] private float throwAirDrag = 5;


    // Setup the original position
    void Start() {

        rigidBody = GetComponent<Rigidbody2D>();

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
                        deltaX = touchPos.x - transform.position.x;
                        deltaY = touchPos.y - transform.position.y;
                        moveAllowed = true;
                        GetComponent<CircleCollider2D>().sharedMaterial = null;

                        rigidBody.gravityScale = 0;
                        rigidBody.drag = throwAirDrag;
                    }
                    break;

                // While the touch is moving and is allowed to move, update the object position
                // to the touchs position
                case TouchPhase.Moved:
                    if (moveAllowed) {
                        Vector2 forceVector = (touchPos - transform.position) * throwBonus;
                        rigidBody.AddForce(forceVector);
                        //transform.position = (new Vector3(touchPos.x - deltaX, touchPos.y - deltaY));
                    }
                    break;

                case TouchPhase.Stationary:
                    if (moveAllowed) {
                        Vector2 forceVector = (touchPos - transform.position) * throwBonus;
                        rigidBody.AddForce(forceVector);
                        //transform.position = (new Vector3(touchPos.x - deltaX, touchPos.y - deltaY));
                    }
                    break;

                // If the touch ended(player let go off the screen), set moveAllowed to false
                case TouchPhase.Ended:
                    //Touch Dropped !!!!!!!!!!!!!!!!!!!

                    if (moveAllowed) {
                        moveAllowed = false;

                        rigidBody.gravityScale = 3;
                        rigidBody.drag = 0;
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
                rigidBody.gravityScale = 0;
                rigidBody.drag = throwAirDrag;
            }

            // If mouseMoveAllowed is true, set the object to follow the mouse until the mouse button is released
            if (mouseMoveAllowed) {
                mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;

                //transform.position = mousePosition;

                Vector2 forceVector = (mousePosition - transform.position) * throwBonus;
                rigidBody.AddForce(forceVector);

                if (Input.GetMouseButtonUp(0)) {
                    rigidBody.gravityScale = 3;
                    rigidBody.drag = 0;
                    mouseMoveAllowed = false;
                }
            }
        }

     } 

}