using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

    [SerializeField] private ImageController imgController;
    [SerializeField] private LetterController letterController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (imgController.getSelectedImgCardInfo() == letterController.getSelectedLetterCardInfo()) {
            imgController.haveMatched();
            letterController.haveMatched();
        }
    }
}
