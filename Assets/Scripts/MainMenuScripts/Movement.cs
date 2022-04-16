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

        float speed = moveSpeed * Time.deltaTime;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            target = cam.ScreenToWorldPoint(touch.position);      
        }

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.x,transform.position.y), speed);
        if(target.x > transform.position.x){
            dirX = 1f;
        }
        else if(target.x < transform.position.x){
            dirX = -1f;
        }
        
	}

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


	void OnTriggerEnter2D (Collider2D col)
	{
  
        if((target.y - transform.position.y) > 1){

            switch (col.tag) {

                case "Jump":
                    rb.AddForce (Vector2.up * 250);
                    break;

            }

        }
			
	}
}
