using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {

    private float deltaX, deltaY;
    private bool moveAllowed = false;
    private bool mouseMoveAllowed = false;
    private Vector3 originalPos;
    [SerializeField] private GameObject[] snapPoints;


    public enum weather{snow, sunny,rainy};
    public weather chosenWeather;

    public enum clothing{jacket, pants, hat, shoes};
    public clothing chosenClothing;


    // Setup the original position
    void Start(){
        originalPos = transform.position;
    }
 
    // Update is called once per frame
    void Update () {

        // If there were any touches on the screen
        if(Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);

            // Get the position of the touch
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);


            switch (touch.phase) {
                // At the moment the screen was touched, look if the touch is on the object.
                // If the touch was on the object, set moveAllowed to true
                case TouchPhase.Began:
                    if (GetComponent<Collider2D> () == Physics2D.OverlapPoint(touchPos)){
                            deltaX = touchPos.x - transform.position.x;
                            deltaY = touchPos.y - transform.position.y;
                            moveAllowed = true;
                            GetComponent<CircleCollider2D>().sharedMaterial = null;
                    }
                    break;

                // While the touch is moving and still on the object, update the object position
                // to the touchs position
                case TouchPhase.Moved:
                    if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos) && moveAllowed)
                        transform.position = (new Vector3(touchPos.x - deltaX, touchPos.y - deltaY));
                    break;

                // If the touch ended(player let go off the screen), set moveAllowed to false
                case TouchPhase.Ended:
                    snapToPoint(touchPos);
                    moveAllowed = false;
                    break;

                }
        }

        // For mouse controls
        else{
            // Get the mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // If the mouse is pressed down and the mouse is over the object, set mouseMoveAllowed to true
            if(Input.GetMouseButtonDown(0) && GetComponent<Collider2D> () == Physics2D.OverlapPoint(mousePosition)){
                mouseMoveAllowed = true;
            }

            // If mouseMoveAllowed is true, set the object to follow the mouse until the mouse button is released
            if(mouseMoveAllowed){
                mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
                transform.position = mousePosition;
                if(Input.GetMouseButtonUp(0)){
                    snapToPoint(mousePosition);
                    mouseMoveAllowed = false;
                }
            }
            
        }
    }


    // Method for snapping to object to a point close to it
    private void snapToPoint(Vector3 position){
        
        bool snapped = false;
        string neededMatch = "";

        // Get what body part this clothing goes on
        switch (chosenClothing){
            case clothing.jacket:
                neededMatch = "Chest";
                break;
            case clothing.pants:
                neededMatch = "Legs";
                break;
            case clothing.hat:
                neededMatch = "Head";
                break;
            case clothing.shoes:
                neededMatch = "Feet";
                break;
            default:
                Debug.Log("Not a valid clothing");
                break;
        
        }

        // Check for each point if the object is close to it
        foreach(GameObject snapPoint in snapPoints){
            Vector3 snapPos = snapPoint.transform.position;
            // If the object is close to the snapPoint and it is the correct body part, set the objects position to that point
            if(GetComponent<Collider2D>() == Physics2D.OverlapCircle(snapPos, 1) && !snapped){
                if(snapPoint.name == neededMatch){
                    transform.position = snapPos;
                    transform.SetParent(snapPoint.transform);
                    snapped = true;
                }
            }
        }

        // If we did not snap to anything, return the object to the original position
        if(!snapped){
            transform.SetParent(null);
            transform.position = originalPos;
        }

    }

}