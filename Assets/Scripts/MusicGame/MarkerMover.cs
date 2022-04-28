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

    [SerializeField] private Transform playerSpritePos;

    // The navigation script of the player
    private Navigate playerNavigateScript;

    private Vector2 defaultSpritePos;

    // Start is called before the first frame update
    void Start()
    {
        // Find navigate script
        playerNavigateScript = player.GetComponent<Navigate>();

        defaultSpritePos = playerSpritePos.transform.localPosition;
    }

    // Teleports the marker/player to a jump point depending on provided index
    public void TeleportToDestination(int index)
    {
        // If marker is at current position don't teleport
        if (!((Vector2) player.transform.position == (Vector2) jumpNodes[index].transform.position))
        {
            // Teleport to a jump point based on provided index
            playerNavigateScript.ai.Teleport(jumpNodes[index].transform.position, false);
            player.transform.position = jumpNodes[index].transform.position;


            if ((Vector2) playerSpritePos.transform.localPosition != defaultSpritePos)
            {
                playerSpritePos.transform.localPosition = defaultSpritePos;
            }
        }
    }

    // Sets a new jump point destination based on provided index.
    public void SetNewDestination(int index)
    {
        // Updates path of the AI script
        playerNavigateScript.setNewPath(jumpNodes[index].transform.position);
    }

    // Teleports the marker/player to the first element in the jump point array
    public void ResetPlayer()
    {
        SetNewDestination(0);
        
        TeleportToDestination(0);
        
        playerSpritePos.transform.localPosition = defaultSpritePos;
    }
}
