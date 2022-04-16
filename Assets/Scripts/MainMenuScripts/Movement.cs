using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{


	[SerializeField]
	float moveSpeed = 3f;

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

        cam = Camera.main;

	}
	
	// Update is called once per frame
	void Update () {

        float speed = moveSpeed * Time.deltaTime;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            target = cam.ScreenToWorldPoint(touch.position);      
        }

        //transform.Translate(target * Time.deltaTime, Space.World);

        transform.position = Vector2.MoveTowards(transform.position, target, speed);
        
	}


	void OnTriggerEnter2D (Collider2D col)
	{
		switch (col.tag) {

		case "Jump":
			rb.AddForce (Vector2.up * 600f);
			break;

		}	
	}
}
