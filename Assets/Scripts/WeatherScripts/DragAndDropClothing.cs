using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragAndDropClothing : MonoBehaviour {

    private float deltaX, deltaY;
    private bool moveAllowed = false;
    private bool mouseMoveAllowed = false;
    private Vector3 originalPos;
    private Vector3 targetPosition;
    private Transform originParent;

    private Vector3 lastPosition;
    private Rigidbody2D ridgidBody;
    private SpriteRenderer spriteRen;

    [SerializeField] private GameObject snapPointsParent;
    private GameObject[] snapPoints;
    private BoxCollider2D[] colliders;
    private Vector2 originalColliderOffset;
    private Vector2 originalColliderSize;

    [SerializeField] private Sprite spriteToSwitchTo;
    private Sprite originalSprite;
    [SerializeField] private GameObject pointToSnapToOnSwitch;


    public WeatherController.WeatherTypes chosenWeather;

    public enum clothing{jacket, shirt, pants, hat, shoes, scarf, gloves};
    public clothing chosenClothing;


    // Setup the original position and snapPoints
    void Start(){
        originalPos = transform.position;
        targetPosition = originalPos;
        originParent = transform.parent;
        
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        // Set up the array of snapPoints
        snapPoints = new GameObject[snapPointsParent.transform.childCount];

        for (int i = 0; i < snapPoints.Length; i++)
        {
            snapPoints[i] = snapPointsParent.transform.GetChild(i).gameObject;
        }


        ridgidBody = GetComponent<Rigidbody2D>();
        spriteRen = GetComponent<SpriteRenderer>();
        colliders = GetComponents<BoxCollider2D>();
        originalColliderOffset = colliders[0].offset;
        originalColliderSize = colliders[0].size;
        originalSprite = spriteRen.sprite;

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
                    if (colliders.Length > 1){
                        if (colliders[0] == Physics2D.OverlapPoint(touchPos) || colliders[1] == Physics2D.OverlapPoint(touchPos))
                        {
                            deltaX = touchPos.x - transform.position.x;
                            deltaY = touchPos.y - transform.position.y;
                            spriteRen.sortingOrder++;
                            moveAllowed = true;
                        }

                    }
                    else
                    {
                        if (colliders[0] == Physics2D.OverlapPoint(touchPos))
                        {
                            deltaX = touchPos.x - transform.position.x;
                            deltaY = touchPos.y - transform.position.y;
                            spriteRen.sortingOrder++;
                            moveAllowed = true;
                        }
                    }
                    
                    break;

                // While the touch is moving and is allowed to move, update the object position
                // to the touchs position
                case TouchPhase.Moved:
                    if (moveAllowed){
                        transform.position = (new Vector3(touchPos.x - deltaX, touchPos.y - deltaY,0));
                    }
                    break;

                // If the touch ended(player let go off the screen), set moveAllowed to false
                case TouchPhase.Ended:
                    if (moveAllowed) {
                        snapToPoint(touchPos);
                        moveAllowed = false;
                        spriteRen.sortingOrder--;
                    }
                    break;

            }
        }

        // For mouse controls
        else{
            // Get the mouse position
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (colliders.Length > 1)
            {
                // If the mouse is pressed down and the mouse is over the object, set mouseMoveAllowed to true
                if (Input.GetMouseButtonDown(0) && (colliders[0] == Physics2D.OverlapPoint(mousePosition) || colliders[1] == Physics2D.OverlapPoint(mousePosition)))
                {
                    mouseMoveAllowed = true;
                    spriteRen.sortingOrder++;
                }

            }
            else
            {
                if (Input.GetMouseButtonDown(0) && colliders[0] == Physics2D.OverlapPoint(mousePosition))
                {
                    mouseMoveAllowed = true;
                    spriteRen.sortingOrder++;
                }
            }

            

            // If mouseMoveAllowed is true, set the object to follow the mouse until the mouse button is released
            if(mouseMoveAllowed){
                transform.position = mousePosition;
                if(Input.GetMouseButtonUp(0)){
                    snapToPoint(mousePosition);
                    mouseMoveAllowed = false;
                    spriteRen.sortingOrder--;
                }
            }
            
        }

        addForceToRidgidbody();
    }

    // Remove the object from the snapPoint
    public void removeFromSnapPoint(){
        transform.SetParent(originParent);
        spriteRen.sprite = originalSprite;
        transform.position = lastPosition;
        targetPosition = originalPos;

        if (colliders.Length > 1)
        {
            colliders[1].offset = new Vector2(0, 0);
            colliders[1].size = new Vector2(0, 0);
        }
    }

    // Used for drawing radius from snapPoints
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (GameObject snapPoint in snapPoints)
        {
            Gizmos.DrawSphere(snapPoint.transform.position, 1);
        }
    }


    // Method for snapping to object to a point close to it
    private void snapToPoint(Vector2 position){
        
        bool snapped = false;
        string neededMatch = "";
        

        // Start values for shortest snap point
        GameObject shortestSnapPoint = null;
        

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
            case clothing.scarf:
                neededMatch = "Throat";
                break;
            case clothing.shoes:
                neededMatch = "Feet";
                break;
            case clothing.gloves:
                neededMatch = "Hand";
                break;
            default:
                Debug.Log("Not a valid clothing");
                break;
        
        }

        // Get the snapPoint with the shortest distance from clothing
        shortestSnapPoint = loopThroughSnapPoints(shortestSnapPoint,snapped);

        if (shortestSnapPoint != null)
        {
            // Check if the snapoint match the clothing type
            if (shortestSnapPoint.name == neededMatch)
            {
                removeClothingType(shortestSnapPoint, neededMatch);

                // Special case with jacket being put on
                // If there is no shirt on snapPoint, we are not putting on the jacket
                if (chosenClothing == clothing.jacket && shortestSnapPoint.transform.childCount <= 0) {  
                    // Do nothing
                }
                else
                {
                    // If we need to change the image of clothing object
                    // Add new clothing to specific snapPoint
                    if (spriteToSwitchTo != null) { 
                        spriteChange();
                    }
                    else
                    {
                        // Add on the new clothing
                        targetPosition = shortestSnapPoint.transform.position;
                        transform.SetParent(shortestSnapPoint.transform);
                        
                    }

                    snapped = true;
                    lastPosition = position;

                }
            }
        }
        

        // If we did not snap to anything, return the object to the original position
        if(!snapped){
            spriteRen.sprite = originalSprite;
            transform.SetParent(originParent);
            transform.position = position; // This is to have the right coordinates
            targetPosition = originalPos;

            colliders[0].offset = originalColliderOffset;
            colliders[0].size = originalColliderSize;

            if (colliders.Length > 1)
            {
                colliders[1].offset = new Vector2(0,0);
                colliders[1].size = new Vector2(0, 0);
            }
        }

    }

    private void spriteChange()
    {
        BoxCollider2D newCollider = colliders[1];

        spriteRen.sprite = spriteToSwitchTo;
        targetPosition = pointToSnapToOnSwitch.transform.position;
        transform.SetParent(pointToSnapToOnSwitch.transform);

        colliders[0].offset = new Vector2(-0.4f,0.125f);
        colliders[0].size = new Vector2(1.61f, 1.35f);

        newCollider.offset = new Vector2(4.8f, 0.1168f);
        newCollider.size = new Vector2(1.537f, 1.208f);

    }



    private GameObject loopThroughSnapPoints(GameObject shortestSnapPoint, bool snapped)
    {
        float shortestSnapPointDistance = float.MaxValue;

        // Check for each point if the object is close to it
        foreach (GameObject snapPoint in snapPoints)
        {
            Vector2 snapPos = snapPoint.transform.position;
            // If the object is close to the snapPoint is not already snapped
            if (colliders[0] == Physics2D.OverlapCircle(snapPos, 1) && !snapped)
            {
                float distToSnapPos = Vector2.Distance(transform.position, snapPos);

                // If we found a snappoint with smaller distance update shortest snap point
                if (distToSnapPos < shortestSnapPointDistance)
                {
                    shortestSnapPoint = snapPoint;
                    shortestSnapPointDistance = distToSnapPos;
                }

            }
        }

        return shortestSnapPoint;
    }

    // Remove clothing if of the same type
    private void removeClothingType(GameObject shortestSnapPoint, string neededMatch)
    {
        

       // If the snapPoint has a clothing of same type, remove it
       DragAndDropClothing[] scripts = shortestSnapPoint.GetComponentsInChildren<DragAndDropClothing>();
       foreach (DragAndDropClothing script in scripts){
            if (script.chosenClothing == chosenClothing){
                script.removeFromSnapPoint();
            }

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