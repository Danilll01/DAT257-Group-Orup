using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	[SerializeField]
	float moveSpeed = 3f;
    float dirX;

	Rigidbody2D rb;

	bool facingRight = false;

    private Vector2 target;

    Camera cam ;


	Vector3 localScale;

	// Use this for initialization
	void Start () {
		localScale = transform.localScale;
		rb = GetComponent<Rigidbody2D> ();
        target = transform.position;
        dirX = 1f;
        cam = Camera.main;
	}

    void LateUpdate() {
        CheckWhereToFace();
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
        }
        // If there were any mouse clicks
        else if(Input.GetMouseButtonDown(0)){
            // Get the click position as a world position
            target = cam.ScreenToWorldPoint(Input.mousePosition);
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


    // Called when object enters a collider trigger
	void OnTriggerEnter2D (Collider2D col)
	{
        // If target y position is higher than objects position
        if((target.y - transform.position.y) > 1){

            switch (col.tag) {
                // if tag on the collider is "Jump", add an upwards force to object
                case "Jump":
                    rb.AddForce (Vector2.up * 250);
                    break;

            }

        }
			
	}
}
