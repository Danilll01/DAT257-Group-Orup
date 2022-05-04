using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillCardImages : MonoBehaviour
{

    [SerializeField] private Transform[] imageHolders;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    // Fills the card with the given amount of images
    public void FillCard(int amount, Sprite image) {

        // Activates the right amount object and fills every child with correct image
        for (int i = 0; i < imageHolders.Length; i++) {
            if (i == amount) {
                imageHolders[amount].gameObject.SetActive(true);
                fillCorrectAmount(imageHolders[amount], image);
            } else {
                imageHolders[i].gameObject.SetActive(false);
            }
        }

    }

    // Fills all images in holders child
    private void fillCorrectAmount(Transform holder, Sprite image) {
        for (int i = 0; i < holder.childCount; i++) {
            holder.GetChild(i).GetComponent<Image>().sprite = image;
        }
    } 
}
