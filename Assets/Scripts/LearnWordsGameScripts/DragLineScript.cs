using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragLineScript : MonoBehaviour
{

    private bool moveAllowed = false;
    private bool mouseMoveAllowed = false;
    LineRenderer lineToBeMoved;
    [SerializeField] private LearnWordsGameHandler lineOwner;
    [SerializeField] private BoxCollider2D[] dragToColliders; // Colliders to drag to
    [SerializeField] private GameObject[] matchingObjects;    // The matching canvas object to the colliders
    [SerializeField] private GameObject startBox;             // Starting canvas button 
    [SerializeField] private bool isImage = false;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update()
    {
        // If there were any touches on the screen and
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            // Get the position of the touch
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            

            switch (touch.phase) {
                // At the moment the screen was touched, look if the touch is on the object.
                // If the touch was on the object, set moveAllowed to true
                case TouchPhase.Began:
                    if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos) && startBox.GetComponent<Image>().color != new Color(100f / 255f, 1, 100f / 255f)) {
                        moveAllowed = true;

                        beginDrag(touchPos);
                    }
                    break;

                // While the touch is moving and is allowed to move, update the object position
                // to the touchs position
                case TouchPhase.Moved:
                    if (moveAllowed) {
                        // Changes end position of line
                        lineToBeMoved.SetPosition(1, new Vector3(touchPos.x, touchPos.y, 0));
                    }
                    break;

                // If the touch ended(player let go off the screen), set moveAllowed to false
                case TouchPhase.Ended:
                    if (moveAllowed) {
                        endDrag(touchPos);
                        moveAllowed = false;
                    }
                    break;

            }
        }

        // For mouse controls
        else {
            // Get the mouse position
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // If the mouse is pressed down and the mouse is over the object, set mouseMoveAllowed to true
            if (Input.GetMouseButtonDown(0) && GetComponent<Collider2D>() == Physics2D.OverlapPoint(mousePosition) && startBox.GetComponent<Image>().color != new Color(100f / 255f, 1, 100f / 255f)) {
                // Make sprite visible
                mouseMoveAllowed = true;

                beginDrag(mousePosition);
            }

            // If mouseMoveAllowed is true, set the object to follow the mouse until the mouse button is released
            if (mouseMoveAllowed) {

                // Changes end position of line
                lineToBeMoved.SetPosition(1, new Vector3(mousePosition.x, mousePosition.y, 0));

                if (Input.GetMouseButtonUp(0)) {
                    endDrag(mousePosition);
                    mouseMoveAllowed = false;
                }
            }

        }
    }

    // Gets new line to drag and sets the starting position
    private void beginDrag(Vector2 touchPos) {
        // Gets line from main script
        LineRenderer line = lineOwner.GetAvalibleLine(touchPos);
        lineToBeMoved = line;

        // Sets the startposition and sets the name to not be removed if lines are redrawn
        lineToBeMoved.name = "DragLine";
        lineToBeMoved.SetPosition(0, (Vector2)Camera.main.ScreenToWorldPoint(startBox.transform.position));
        lineToBeMoved.SetPosition(1, new Vector3(touchPos.x, touchPos.y, 0));
    }

    // Ends the drag operation. This means calling the correct method to look a answer VS to just remove the line again
    private void endDrag(Vector2 touchPos) {
        
        // Change back name
        lineToBeMoved.name = "Line";
        for (int i = 0; i < dragToColliders.Length; i++) {
            // If it collides with a collider that is not green
            if (dragToColliders[i] == Physics2D.OverlapPoint(touchPos) && matchingObjects[i].GetComponent<Image>().color != new Color(100f / 255f, 1, 100f / 255f)) {

                // To make sure the line sets up the dictionary right
                if (isImage) {
                    lineOwner.makeGuess(startBox, matchingObjects[i]);
                } else {
                    lineOwner.makeGuess(matchingObjects[i], startBox);
                }

                break;
            }
            // If there was no match, redraw all lines and reset
            lineOwner.reDrawLines();
        }
    }
}
