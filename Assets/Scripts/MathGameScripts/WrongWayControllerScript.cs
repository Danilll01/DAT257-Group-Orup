using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongWayControllerScript : MonoBehaviour
{

    private Camera camera;
    private float timer;

    [SerializeField] private float timeUntillSwitchBack;
    [SerializeField] private TriggerScreenTransition screenTransition;

    // Start is called before the first frame update
    void Start()
    {
        // Gets camera for this screen and sets up the timer for use
        camera = transform.GetComponent<Camera>();
        timer = timeUntillSwitchBack;
    }

    // Update is called once per frame
    void Update()
    {
        if (camera.enabled == true) { // If on this screen decrement timer
            timer -= Time.deltaTime;
        }

        if (timer <= 0) { // When timer reaches zero, switch back to main screen and reset timer
            screenTransition.SwitchBackToMainScreen();
            timer = timeUntillSwitchBack;
        }

    }
}
