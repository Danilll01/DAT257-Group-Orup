using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MarkerMover : MonoBehaviour
{
    // All nodes that the marker/player can jump to
    [SerializeField] private GameObject[] jumpNodes;

    // The player object
    [SerializeField] private GameObject player;

    // The navigation script of the player
    private Navigate playerNavigateScript;

    // Start is called before the first frame update
    void Start()
    {
        // Find navigate script
        playerNavigateScript = player.GetComponent<Navigate>();
    }

    // Teleports the marker/player to a jump point depending on provided index
    public void TeleportToDestination(int index)
    {
        // If marker is at current position don't teleport
        if (!((Vector2) player.transform.position == (Vector2) jumpNodes[index].transform.position))
        {
            // Teleport to a jump point based on provided index
            playerNavigateScript.ai.Teleport(jumpNodes[index].transform.position);
        }
    }

    // Sets a new jump point destination based on provided index.
    public void SetNewDestination(int index)
    {
        // If index is going to be out of bounds, set location to first element in jump point array
        if (index >= (jumpNodes.Length-2))
        {
            index = 0;
        } else
        {
            index++;
        }

        // Updates path of the AI script
        playerNavigateScript.setNewPath(jumpNodes[index].transform.position);
    }

    // Teleports the marker/player to the first element in the jump point array
    public void ResetPlayer()
    {
        TeleportToDestination(0);
    }
}
