using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThermometerControl : MonoBehaviour{

    public Text temperatureText;
    public float updateTemp;

    public Image TempBar;
    public float maxTemp;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){

        temperatureText.text = (int)updateTemp + " *C;";
        TempBar.fillAmount = updateTemp / maxTemp;
        
    }
}
