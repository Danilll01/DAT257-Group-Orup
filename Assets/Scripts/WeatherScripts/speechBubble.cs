using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class speechBubble : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speechText;
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private string message;
    [SerializeField] private AudioSource speech;
    [SerializeField] private Button validateButton;

    // Show the speech bubble and activate text coróutine
    // Also deativate the validateButton temporarily
    public void showBubble()
    {
        this.gameObject.SetActive(true);
        speechText.text = "";
        validateButton.interactable = false;
        StartCoroutine(TypeText());
    }

    // Effect for writing out the text letter by letter
    IEnumerator TypeText()
    {
        // Play the audio clip
        speech.Play();

        foreach (char letter in message.ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(delay);
        }

        // Wait for some seconds before hiding the speech bubble
        yield return new WaitForSeconds(5);
        hideBubble();
    }

    // Hide the speech bubble and activate validateButton again
    private void hideBubble()
    {
        speechText.text = "";
        validateButton.interactable = true;
        this.gameObject.SetActive(false);
    }

}
