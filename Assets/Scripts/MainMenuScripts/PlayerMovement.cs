using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private CharacterController2D controller;
    [SerializeField] private Animator animator;
    [SerializeField] private float runSpeed = 40f;

    private float horizontalMove = 0f;
    private bool jump = false;
    private bool crouch = false;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetKeyDown(KeyCode.Space)) {
            jump = true;
            animator.SetBool("IsJumping", true);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            crouch = true;
            animator.SetBool("IsCrouching", true);
        } else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S)) {
            crouch = false;
            animator.SetBool("IsCrouching", false);
        }
        
    }

    private void FixedUpdate() {
  
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;

    }

    public void OnLandingEvent() {
        animator.SetBool("IsJumping", false);
    }
}
