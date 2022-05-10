using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartMeunu : MonoBehaviour
{
    [SerializeField] private GameObject openStartMenuBtn;
    [SerializeField] private GameObject closeBtn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    // Set hideCloseButton to true if a game mode has not been selected
    public void OpenStartMenu(bool hideCloseButton)
    {
        gameObject.SetActive(true);
        openStartMenuBtn.SetActive(false); // Hide button for opening this start menu when start menu is opened

        // Show close button if not true
        if (!hideCloseButton) {
            closeBtn.SetActive(true); 
        }

    }

    public void CloseStartMenu()
    {
        gameObject.SetActive(false);
        openStartMenuBtn.SetActive(true);// Show button for opening this start menu when start menu is closed

    }
}
