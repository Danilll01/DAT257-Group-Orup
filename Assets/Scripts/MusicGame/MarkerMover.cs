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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMarkerMovement()
    {
        playerNavigateScript.setNewPath((Vector2) endPos.position);
        //playerNavigateScript.setNewPath((Vector2) originalPos.position);
        
    }
}
