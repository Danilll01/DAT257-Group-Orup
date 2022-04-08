using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    public void OnMathGameClick() {
        // This changes scene to be the math game. In file -> build settings you can find what number you should load for the corresponing scene
        SceneManager.LoadScene(1); 
    }

    public void OnWeatherGameClick() {
        // Needs to be added
        SceneManager.LoadScene(2);
    }

    public void OnMemoryGameClick() {
        // Needs to be added
        // SceneManager.LoadScene();
    }

    public void OnLearnWordGameClick() {
        // Needs to be added
        // SceneManager.LoadScene();
    }

    public void OnMusicGameClick() {
        // Needs to be added
        // SceneManager.LoadScene();
    }
}
