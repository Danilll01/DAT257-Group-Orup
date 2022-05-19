using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMeunu : MonoBehaviour
{
    [SerializeField] private GameObject openStartMenuBtn;
    [SerializeField] private GameObject closeBtn;
    [SerializeField] private GameObject randomBtn;
    [SerializeField] private Button transparentBackgroundButton;

    
    // Set hideCloseButton to true if a game mode has not been selected
    public void OpenStartMenu(bool hideCloseButton)
    {
        gameObject.SetActive(true);
        openStartMenuBtn.SetActive(false); // Hide button for opening this start menu when start menu is opened

        // Show close button if not true
        if (!hideCloseButton) {
            closeBtn.SetActive(true);
            transparentBackgroundButton.interactable = true;
        } else
        {
            transparentBackgroundButton.interactable = false;
        }

    }

    public void CloseStartMenu()
    {
        gameObject.SetActive(false);
        openStartMenuBtn.SetActive(true); // Show button that opens up this start menu again

    }
}
