using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

using TMPro;
using UnityEngine.SceneManagement;

public class PortalHandler : MonoBehaviour
{

    // List of nodes close to each portal
    [SerializeField] private Transform[] portals;
    [SerializeField] private AudioClip[] portalSound;
    // The player object
    [SerializeField] private Navigate navigation;


    private AudioSource audioSource;




    public void onPortalClick(int portalId)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(portalSound[portalId - 1]);

        // Make player go to the portal's corresponding node
        //navigation.setNewPath(portals[portalId].position);
        navigation.MoveTowardsPortal(portals[portalId-1].position,
            
            () => {

                audioSource.Play();
                StorePortal.setLastPortalSceneIndex(portalId);
                SceneManager.LoadScene(portalId);

            });

    }


    // Start is called before the first frame update
    void Start()
    {

        audioSource = GetComponent<AudioSource>();

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
