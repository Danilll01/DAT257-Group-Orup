using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PortalHandler : MonoBehaviour
{

    [SerializeField] private Transform[] portals;
    [SerializeField] private Navigate navigation;


    public void onPortalClick(int portalId)
    {
        // kalla på navigate 
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
