using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Keeps track of the last portal visited. 
public static class StorePortal
{
    // last visited portals scene index, default is MainMenu scene index
    private static int lastPortalSceneIndex = 0;

    public static void setLastPortalSceneIndex (int sceneIndex)
    {
        // check so that it is one of the games
        if ((sceneIndex >= 0 && sceneIndex <= 5))
        {
            lastPortalSceneIndex = sceneIndex;
        }
    }

    public static int getLastPortalSceneIndex()
    {
        return lastPortalSceneIndex;
    }

}
