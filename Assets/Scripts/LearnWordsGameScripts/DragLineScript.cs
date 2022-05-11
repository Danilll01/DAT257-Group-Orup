using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragLineScript : MonoBehaviour
{

    private bool moveAllowed = false;
    private bool mouseMoveAllowed = false;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update()
    {
        // If there were any touches on the screen
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            // Get the position of the touch
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);


            switch (touch.phase) {
                // At the moment the screen was touched, look if the touch is on the object.
                // If the touch was on the object, set moveAllowed to true
                case TouchPhase.Began:
                    if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)) {
                        moveAllowed = true;
                        
                    }
                    break;

                // While the touch is moving and is allowed to move, update the object position
                // to the touchs position
                case TouchPhase.Moved:
                    if (moveAllowed) {
                        transform.position = new Vector3(touchPos.x, touchPos.y);
                    }
                    break;

                // If the touch ended(player let go off the screen), set moveAllowed to false
                case TouchPhase.Ended:
                    // Only move object if it should be moved
                    if (moveAllowed) {
                        
                    }
                    moveAllowed = false;
                    break;

            }
        }

        // For mouse controls
        else {
            // Get the mouse position
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // If the mouse is pressed down and the mouse is over the object, set mouseMoveAllowed to true
            if (Input.GetMouseButtonDown(0) && GetComponent<Collider2D>() == Physics2D.OverlapPoint(mousePosition)) {
                // Make sprite visible
                

                mouseMoveAllowed = true;
            }

            // If mouseMoveAllowed is true, set the object to follow the mouse until the mouse button is released
            if (mouseMoveAllowed) {
                //transform.position = mousePosition;
                
                if (Input.GetMouseButtonUp(0)) {
                    

                    mouseMoveAllowed = false;
                }
            }

        }
    }
}
