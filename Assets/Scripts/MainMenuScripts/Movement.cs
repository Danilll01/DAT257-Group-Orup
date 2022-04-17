using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	[SerializeField]
	float moveSpeed = 3f;

    [SerializeField]
    float jumpForce = 250f;
    float dirX;

	Rigidbody2D rb;

	bool facingRight = false;

    private Vector2 target;
    private Vector2 storedPoint;

    [SerializeField]
    private Transform[] helperPoints;

    private bool helping;

    RaycastHit hit;

    Camera cam;

    [SerializeField] private Animator animator;


	Vector3 localScale;

	// Use this for initialization
	void Start () {
		localScale = transform.localScale;
		rb = GetComponent<Rigidbody2D> ();
        target = transform.position;
        dirX = 1f;
        helping = false;
        cam = Camera.main;
	}

    void LateUpdate() {
        CheckWhereToFace();
    }

    void OnEnable(){
        target = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        // Get the speed not based on current FPS
        float speed = moveSpeed * Time.deltaTime;

        // If there were any input touches
        if (Input.touchCount > 0)
        {
            // Get the touch position as a world position
            Touch touch = Input.GetTouch(0);
            target = cam.ScreenToWorldPoint(touch.position);    
            ShootRay();

            
        }
        // If there were any mouse clicks
        else if(Input.GetMouseButtonDown(0)){
            // Get the click position as a world position
            target = cam.ScreenToWorldPoint(Input.mousePosition);
            ShootRay();
        }

        // If the target y position is two floors up, set target to
        // first floor first, then go back to second floor
        if(target.y - transform.position.y > 3){
            storedPoint = target;
            helping = true;

            Transform closest = helperPoints[0];
            foreach (Transform point in helperPoints)
            {
                float diffClosest = Mathf.Abs(closest.position.x - transform.position.x);
                float diffPoint = Mathf.Abs(point.position.x - transform.position.x);
                if(diffPoint < diffClosest){
                    closest = point;
                }
            }
            target = closest.transform.position;
        }

        if(helping && transform.position.x == target.x){
            target = storedPoint;
            helping = false;
        }

        // Move the object towards the target position on the x axis.
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.x,transform.position.y), speed);

        // Check what way the object is going
        if(target.x > transform.position.x){
            dirX = 1f;
        }
        else if(target.x < transform.position.x){
            dirX = -1f;
        }
        
	}

    // Flips the object to face the direction it is going
    void CheckWhereToFace()
	{
		if (dirX > 0)
			facingRight = true;
		else if (dirX < 0)
			facingRight = false;

		if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
			localScale.x *= -1;

		transform.localScale = localScale;
	}

    void ShootRay(){
        // Sends a ray down to get point on an object
        RaycastHit2D hit = Physics2D.Raycast(target, -Vector2.up);  
        target = hit.point;
    }


    // Called when object enters a collider trigger
	void OnTriggerEnter2D (Collider2D col)
	{
        float diff = target.x - transform.position.x;
        // If target y position is higher than objects position and the target
        // position is close
        if((target.y - transform.position.y) > 1 && Mathf.Abs(diff) < 3){
            // if we are going in the right direction
            if((facingRight && diff > 0) || (!facingRight && diff < 0)){
                switch (col.tag) {
                // if tag on the collider is "Jump", add an upwards force to object
                // a.k.a jump
                case "Jump":
                    rb.AddForce (Vector2.up * jumpForce);
                    animator.SetBool("IsJumping", true);
                    break;

                }

            }

        }
			
	}

    public void OnLandingEvent() {
        animator.SetBool("IsJumping", false);
    }
}
