using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioClip;

    private CircleCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = this.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // If there were any touches on the screen
        // Send to touchMovement function
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchClick(touch);

        }

        // For mouse controls
        else
        {

           Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
           mouseClick(mousePosition);

        }
    }

    private void touchClick(Touch touch)
    {
        // Get the position of the touch
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                // If there was a click on the screen on the fox, play the soound
                if (collider == Physics2D.OverlapPoint(touchPos))
                {
                    audioClip.Play();
                }
            break;
        }
    }

    // If there was a click on the screen on the fox, play the soound
    private void mouseClick(Vector2 mousePosition)
    {
        if (Input.GetMouseButtonDown(0) && (collider == Physics2D.OverlapPoint(mousePosition)))
        {
            audioClip.Play();
        }
    }
}
