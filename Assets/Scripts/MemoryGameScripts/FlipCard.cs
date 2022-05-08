using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCard : MonoBehaviour
{
    private void Flip(float degrees)
    {
        transform.eulerAngles = new (0, degrees, 0); 
    }

    public void FlipCardToOriginalState()
    {
        Flip(0);
    }

    public void FlipCardToAnimalState()
    {
        Flip(180);
    }
}
