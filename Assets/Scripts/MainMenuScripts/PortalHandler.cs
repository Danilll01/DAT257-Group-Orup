using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

using TMPro;

public class PortalHandler : MonoBehaviour
{

    [SerializeField] private Transform[] portals;
    [SerializeField] private Navigate navigation;
    private Transform startPosition;


    public void onPortalClick(int portalId)
    {
        // Make player go to the portal's corresponding node
        navigation.setNewPath(portals[portalId].position);

    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
