using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryJump : MonoBehaviour
{
    [SerializeField] private JumpNodeScript jumpNode;

    public void Jump() {
        jumpNode.StartJumpAnimation(transform.GetChild(0), transform.GetChild(0).transform.localPosition,
            () => {
               
            });
    }


}
