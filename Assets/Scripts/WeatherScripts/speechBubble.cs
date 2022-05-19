using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class speechBubble : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speechText;
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private AudioSource[] speeches;
    [SerializeField] private Button validateButton;
    [SerializeField] private Button doorButton;

    // Show the speech bubble and activate text coróutine
    // Also deativate the validateButton temporarily
    public void showBubble(string message, int speechIndex)
    {
        this.gameObject.SetActive(true);
        speechText.text = "";
        validateButton.interactable = false;
        doorButton.interactable = false;
        StartCoroutine(TypeText(message, speechIndex));
    }

    // Effect for writing out the text letter by letter
    IEnumerator TypeText(string message, int speechIndex)
    {
        // Play the audio clip
        speeches[speechIndex].Play();

        foreach (char letter in message.ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(delay);
        }

        // Wait for some seconds before hiding the speech bubble
        yield return new WaitForSeconds(2);
        hideBubble();
    }

    // Hide the speech bubble and activate validateButton again
    private void hideBubble()
    {
        speechText.text = "";
        validateButton.interactable = true;
        doorButton.interactable = true;
        this.gameObject.SetActive(false);
    }

}
