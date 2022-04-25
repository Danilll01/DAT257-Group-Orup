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

    public Vector2 JumpFromPosition() {
        return transform.position;
    }

    public Vector2 JumpToCoordinates() {
        return jumpToNode.position;
    }

    public float GetJumpSpeed() {
        return Vector2.Distance(transform.position, jumpToNode.position) / jumpTime;
    }

    public void StartJumpAnimation(Transform agent, Action setNormalAgentSpeed) {
        if (!isJumping) {
            isJumping = true;
            StartCoroutine(Curve(agent, jumpTime, setNormalAgentSpeed));
        }
    }




    private IEnumerator Curve(Transform agent, float duration, Action setNormalAgentSpeed) {
        Vector3 normalValues = agent.localPosition;

        
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f) {
            float yOffset = jumpCurve.Evaluate(normalizedTime);

            agent.transform.localPosition = normalValues + (yOffset * Vector3.up);
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
        setNormalAgentSpeed();
        agent.localPosition = normalValues;
        isJumping = false;
    }
}
