using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongWayControllerScript : MonoBehaviour {

    private Camera cam;

    [SerializeField] private float timeUntillSwitchBack;
    [SerializeField] private TriggerScreenTransition screenTransition;

    [SerializeField] private float playerSpeed;
    [SerializeField] private Transform player;
    [SerializeField] private Transform moveTo;

    private Vector2 startingLocation;
    private bool haveStartedAnimation = false;


    // Start is called before the first frame update
    void Start() {
        // Gets camera for this screen and sets up the timer for use
        cam = transform.GetComponent<Camera>();

        // Sets the starting location for the animation player
        startingLocation = player.position;
    }

    // Update is called once per frame
    void Update() {
        if (cam.enabled == true && !haveStartedAnimation) { // If on this screen animate player
            player.position = startingLocation;
            haveStartedAnimation = true;
            StartCoroutine(runPlayerAnimation());
        }
    }

    // Runs the player animation untill callback
    private IEnumerator runPlayerAnimation() {

        float timeUntilCallback = 0; // Value to know how long untill callback

        // Move towards the moveTo position
        while (Vector2.Distance(player.position, moveTo.position) > 0.05f) {
            player.position = Vector2.MoveTowards(player.position, moveTo.position, playerSpeed * Time.deltaTime);
            timeUntilCallback += Time.deltaTime;
            yield return null;
        }

        // Stands at position untill moveback
        float timer = 0;
        while (timer <= timeUntillSwitchBack) {
            timer += Time.deltaTime;
            yield return null;
        }

        // The vector to move back with
        Vector2 moveBackVector = Vector2.MoveTowards(player.position, startingLocation, playerSpeed * Time.deltaTime) - (Vector2) player.position;

        // Moves back until the time for callback has come
        while (timeUntilCallback > 0) {
            timeUntilCallback -= Time.deltaTime;
            player.position += (Vector3) moveBackVector;
            yield return null;
        }

        // Calls back
        screenTransition.SwitchBackToMainScreen();

        // Moves until screen transition is complete
        while (cam.enabled == true) {
            player.position += (Vector3) moveBackVector;
            yield return null;
        }

        // Resets itself
        haveStartedAnimation = false;
    }
}
