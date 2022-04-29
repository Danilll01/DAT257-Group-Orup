using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalCollision : MonoBehaviour
{

    [SerializeField] private int sceneIndex;

    // If collider is triggered, open its corresponding game's scene 
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            gameObject.transform.GetComponent<BoxCollider2D>().isTrigger = false;
            // Change to game's scene
            SceneManager.LoadScene(sceneIndex);

        }
        

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
