using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

using TMPro;

public class PortalHandler : MonoBehaviour
{

    // List of nodes close to each portal
    [SerializeField] private Transform[] portals;
    // The player object
    [SerializeField] private Navigate navigation;
    [SerializeField] private Collider2D[] colliders;




    public void onPortalClick(int portalId)
    {
        // Make player go to the portal's corresponding node
        navigation.setNewPath(portals[portalId].position);
        foreach (Collider2D collider in colliders)
        {
            collider.isTrigger = false;
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        // if player(navigation) is coming from a portal, set its position to this portals node
        if (!(StorePortal.getLastPortalSceneIndex() == 0))
        {
            Transform startPos = portals[getPortalsIndex(StorePortal.getLastPortalSceneIndex())];
            navigation.transform.position = startPos.position;
        }
        
    }

    // Translates scene index of the game to its index in portals 
    private int getPortalsIndex(int sceneIndex)
    {
        return (sceneIndex - 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
