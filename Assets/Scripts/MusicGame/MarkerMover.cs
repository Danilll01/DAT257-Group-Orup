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

    // Position of the player sprite
    [SerializeField] private Transform playerSpritePos;

    // The navigation script of the player
    private Navigate playerNavigateScript;

    // Used to correctly reset the player sprite
    private Vector2 defaultSpritePos;

    // Start is called before the first frame update
    void Start()
    {
        // Find navigate script
        playerNavigateScript = player.GetComponent<Navigate>();

        // Get the default position of the sprite
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

            // Reset player sprite position
            playerSpritePos.localPosition = defaultSpritePos;
            
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
        
        // Set sprite to correct position
        playerSpritePos.transform.localPosition = defaultSpritePos;
    }

    // Changes the time between jumps on all jump nodes
    public void ChangeJumpTime(float time)
    {
        foreach (GameObject node in jumpNodes)
        {
            node.GetComponent<JumpNodeScript>().SetJumpTime(time);
        }
    }
}
