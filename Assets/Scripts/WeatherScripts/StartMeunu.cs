using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartMeunu : MonoBehaviour
{

    [SerializeField] private GameObject closeBtn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenStartMenu(bool firstTime)
    {
        gameObject.SetActive(true);
        if (!firstTime) {
            closeBtn.SetActive(true);
        }

    }

    public void CloseStartMenu()
    {
        gameObject.SetActive(false); 

    }
}
