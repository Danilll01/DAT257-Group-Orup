using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MarkerMover : MonoBehaviour
{

    [SerializeField] private GameObject[] jumpNodes;
    [SerializeField] private GameObject playerWrapper;

    [SerializeField] private Transform originalPos;
    [SerializeField] private Transform endPos;

    private Navigate playerNavigateScript;

    // Start is called before the first frame update
    void Start()
    {
        playerNavigateScript = playerWrapper.GetComponent<Navigate>();
    }

    public void TeleportPlayerToCurrentDestination(int index)
    {
        if (!((Vector2) playerWrapper.transform.position == (Vector2) jumpNodes[index].transform.position))
        {
            playerNavigateScript.ai.Teleport(jumpNodes[index].transform.position);
        }
    }

    public void SetNewDestination(int index)
    {
        if (index >= (jumpNodes.Length-2))
        {
            index = 0;
        } else
        {
            index++;
        }

        playerNavigateScript.setNewPath(jumpNodes[index].transform.position);
    }

    public void ResetPlayer()
    {
        TeleportPlayerToCurrentDestination(0);
    }
}
