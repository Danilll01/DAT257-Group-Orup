using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpNodeScript : MonoBehaviour
{

    [SerializeField] private Transform jumpToNode;
    [SerializeField] private AnimationCurve jumpCurve = new AnimationCurve();
    [SerializeField] private float jumpTime = 0.5f;

    private bool isJumping;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    // Returns the position to jump from
    public Vector2 JumpFromPosition() {
        return transform.position;
    }

    // Returns the position to jump to from this node
    public Vector2 JumpToCoordinates() {
        return jumpToNode.position;
    }

    // Calculate and return the speed the agent have to has to be able to do the jump in the given time
    public float GetJumpSpeed() {
        return Vector2.Distance(transform.position, jumpToNode.position) / jumpTime;
    }

    public void SetJumpTime(float time)
    {
        jumpTime = time;
    }

    // Starting function for the jump animation, blocks repeated calls if animation is playing
    public void StartJumpAnimation(Transform agent, Action setNormalAgentSpeed) {
        if (!isJumping) {
            isJumping = true;
            StartCoroutine(Curve(agent, jumpTime, setNormalAgentSpeed));
        }
    }

    // Playes the jump animation where agent is the player sprite, duration is to know for how long to play, and the action is a callback to reset the agent speed
    private IEnumerator Curve(Transform agent, float duration, Action setNormalAgentSpeed) {
        
        // Saves the localPosition of player sprite
        Vector3 normalValues = agent.localPosition;
        
        // Runs the animation over several frames
        float normalizedTime = 0.0f;

        // Gets the animator
        Animator animator = agent.GetComponent<Animator>();
        animator.SetBool("StartJump", true);

        while (normalizedTime < 1.0f) {
            float yOffset = jumpCurve.Evaluate(normalizedTime); // Evaluates the curve based on current time in animation

            agent.transform.localPosition = normalValues + (yOffset * Vector3.up); // Change local sprite position to move the sprite
            normalizedTime += Time.deltaTime / duration;

            if (normalizedTime > 0.5f) {
                animator.SetBool("EndJump", true);
            }

            yield return null;
        }
        setNormalAgentSpeed();
        agent.localPosition = normalValues; // Return player sprite to it's original location
        animator.SetBool("StartJump", false);
        animator.SetBool("EndJump", false);
        isJumping = false;
    }
}
