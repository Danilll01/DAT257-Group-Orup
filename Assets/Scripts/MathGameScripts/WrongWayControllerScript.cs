using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongWayControllerScript : MonoBehaviour
{

    private Camera camera;
    private float timer;

    [SerializeField] private float timeUntillSwitchBack;
    [SerializeField] private TriggerScreenTransition screenTransition;

    [SerializeField] private float playerSpeed;
    [SerializeField] private Transform player;
    [SerializeField] private Transform moveTo;
    private Vector2 startingLocation;
    private Vector2 currentTarget;
    private bool haveSentScreenSwitch = true;


    // Start is called before the first frame update
    void Start()
    {
        // Gets camera for this screen and sets up the timer for use
        camera = transform.GetComponent<Camera>();
        timer = timeUntillSwitchBack;

        // Sets the starting location for the animation player
        startingLocation = player.position;
        currentTarget = startingLocation;
    }

    // Update is called once per frame
    void Update()
    {
        if (camera.enabled == true) { // If on this screen animate player

            // Set target to move towards
            if (Vector2.Distance(player.position, startingLocation) < 0.1 && haveSentScreenSwitch) {
                currentTarget = moveTo.position;
            }
            
            // Wait at target position for given time
            if (Vector2.Distance(player.position, moveTo.position) < 0.1) {
                timer -= Time.deltaTime;
                haveSentScreenSwitch = false;
            }

            // Start moving back when timer reached zero
            if (timer <= 0) { 
                currentTarget = startingLocation;
            }

            // Switch camera back and reset everyting
            if (Vector2.Distance(player.position, startingLocation) < 0.05 && !haveSentScreenSwitch) {
                screenTransition.SwitchBackToMainScreen();
                haveSentScreenSwitch = true;
                timer = timeUntillSwitchBack;
            }

            // Move player towards target position
            player.position = Vector2.MoveTowards(player.position, currentTarget, playerSpeed * Time.deltaTime);
        }
    }
}
