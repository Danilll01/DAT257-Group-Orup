using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalHandler : MonoBehaviour
{

    // List of nodes close to each portal
    [SerializeField] private Transform[] portals;
    // The player object
    [SerializeField] private Navigate navigation;

    [SerializeField] private Image[] names;

    private int portalSelected = 0;

    public void onPortalClick(int portalId)
    {
        // Make player go to the portal's corresponding node
        //navigation.setNewPath(portals[portalId].position);

        navigation.MoveTowardsPortal(portals[portalId-1].position,
            
            () => {

                StorePortal.setLastPortalSceneIndex(portalId);
                SceneManager.LoadScene(portalId);

            });

        updateColor(portalId);


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

    // Make the color darker on the selected portals name image
    // Bring back the color on the previous selected image
    private void updateColor(int portalId)
    {
        Image imageOld = names[portalSelected].GetComponent<Image>();
        imageOld.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 1f);
        portalSelected = portalId - 1;
        Image imageNew = names[portalSelected].GetComponent<Image>();
        imageNew.color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 1f);
    }
}
