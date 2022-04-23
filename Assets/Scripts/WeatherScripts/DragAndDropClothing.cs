using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragAndDropClothing : MonoBehaviour {

    private float deltaX, deltaY;
    private bool moveAllowed = false;
    private bool mouseMoveAllowed = false;
    private Vector3 originalPos;
    private Vector3 targetPosition;

    private Vector3 lastPosition;
    private Rigidbody2D ridgidBody;

    [SerializeField] private GameObject snapPointsParent;
    private GameObject[] snapPoints;


    public WeatherController.WeatherTypes chosenWeather;

    public enum clothing{jacket,shirt, pants, hat, shoes};
    public clothing chosenClothing;


    // Setup the original position and snapPoints
    void Start(){
        originalPos = transform.position;
        targetPosition = originalPos;

        snapPoints = new GameObject[snapPointsParent.transform.childCount];

        for (int i = 0; i < snapPoints.Length; i++)
        {
            snapPoints[i] = snapPointsParent.transform.GetChild(i).gameObject;
        }


        ridgidBody = GetComponent<Rigidbody2D>();

        // Dissables collision between clothes objects 
        Physics2D.IgnoreLayerCollision(6, 6); // Clothes needs to be on layer 6

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

                // While the touch is moving and is allowed to move, update the object position
                // to the touchs position
                case TouchPhase.Moved:
                    if (moveAllowed){
                        transform.position = (new Vector3(touchPos.x - deltaX, touchPos.y - deltaY));
                    }
                    break;

                // If the touch ended(player let go off the screen), set moveAllowed to false
                case TouchPhase.Ended:
                    if (moveAllowed) {
                        snapToPoint(touchPos);
                        moveAllowed = false;
                    }
                    break;

                }
        }

        // For mouse controls
        else{
            // Get the mouse position
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // If the mouse is pressed down and the mouse is over the object, set mouseMoveAllowed to true
            if(Input.GetMouseButtonDown(0) && GetComponent<Collider2D> () == Physics2D.OverlapPoint(mousePosition)){
                mouseMoveAllowed = true;
            }

            // If mouseMoveAllowed is true, set the object to follow the mouse until the mouse button is released
            if(mouseMoveAllowed){
                transform.position = mousePosition;
                if(Input.GetMouseButtonUp(0)){
                    snapToPoint(mousePosition);
                    mouseMoveAllowed = false;
                }
            }
            
        }

        addForceToRidgidbody();
    }

    // Remove the object from the snapPoint
    public void removeFromSnapPoint(){
        transform.SetParent(null);
        transform.position = lastPosition;
        targetPosition = originalPos;
    }


    // Method for snapping to object to a point close to it
    private void snapToPoint(Vector2 position){
        
        bool snapped = false;
        string neededMatch = "";
        

        // Start values for shortest snap point
        GameObject shortestSnapPoint = snapPoints[0];
        float shortestSnapPointDistance = float.MaxValue;

        // Get what body part this clothing goes on
        switch (chosenClothing){
            case clothing.jacket: case clothing.shirt:
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
            Vector2 snapPos = snapPoint.transform.position;
            // If the object is close to the snapPoint is not already snapped
            if(GetComponent<Collider2D>() == Physics2D.OverlapCircle(snapPos, 1) && !snapped){
                float distToSnapPos = Vector2.Distance(transform.position,snapPos);
            
                // If we found a snappoint with smaller distance update shortest snap point
                if (distToSnapPos < shortestSnapPointDistance)
                {
                    shortestSnapPoint = snapPoint;
                    shortestSnapPointDistance = distToSnapPos;
                }
                
            }
        }

        // Check if the snapoint match the clothing type
        if(shortestSnapPoint.name == neededMatch){

            // If the snapPoint has a clothing of same type, remove it
            DragAndDropClothing[] scripts = shortestSnapPoint.GetComponentsInChildren<DragAndDropClothing>();
            foreach (DragAndDropClothing script in scripts)
            {
                if (script.chosenClothing == chosenClothing)
                {
                    script.removeFromSnapPoint();
                }

            }
  
            // Add on the new clothing
            targetPosition = shortestSnapPoint.transform.position;
            transform.SetParent(shortestSnapPoint.transform);
            snapped = true;
            lastPosition = position;

        }

        // If we did not snap to anything, return the object to the original position
        if(!snapped){
            transform.SetParent(null);
            transform.position = position; // This is to have the right coordinates
            targetPosition = originalPos;
        }

    }

    // Adds force to the player ridgidbody to move towards the target point
    private void addForceToRidgidbody() {
        if (!mouseMoveAllowed && !moveAllowed) {
            Vector2 forceVector = (targetPosition - transform.position) * 15 * (Time.deltaTime * 1000);
            ridgidBody.AddForce(forceVector);
        }
    }

}