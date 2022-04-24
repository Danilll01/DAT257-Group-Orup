using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closetInventory : MonoBehaviour
{
    List<GameObject> clothes;

    int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
        
        // Initialize our array of clothing items
        clothes = new List<GameObject>(this.transform.childCount);

        for (int i = 0; i < this.transform.childCount; i++)
        {
            clothes.Add(this.transform.GetChild(i).gameObject);
            if(i > 0){
                clothes[i].SetActive(false);
            }
        }
       
    }

    // When a clothing is removed from closet, remove it from array and show next item
    public void removeClothingFromArray(GameObject obj, bool switching){
        if(clothes.Contains(obj)){
            clothes.Remove(obj);
            currentIndex++;
            if(currentIndex >= clothes.Count || currentIndex < 0){
                currentIndex = 0;
            }

            if(clothes.Count > 0 && !switching){
                clothes[currentIndex].SetActive(true);
            }
            else if (switching){
                currentIndex = clothes.Count -1;
            }
        }
        
    }

    // When clothing is taken off character, add it to our array
    public void AddClothingToArray(GameObject obj, bool switching){
        if(!clothes.Contains(obj)){
            if(clothes.Count > 0 && !switching){   
                clothes[currentIndex].SetActive(false);
            }
 
            clothes.Add(obj);
            
            currentIndex = clothes.IndexOf(obj);
        }
    }


    // Go to the next clothing item
    public void buttonRightPressed(){
        if(clothes.Count > 0){
            clothes[currentIndex].SetActive(false);
            currentIndex++;

            if(currentIndex == clothes.Count){
                currentIndex = 0;
            }

            clothes[currentIndex].SetActive(true);
        }
        
    }

    // Go to the previous clothing item
    public void buttonLeftPressed(){
        if(clothes.Count > 0){
            clothes[currentIndex].SetActive(false);
            currentIndex--;

            if(currentIndex < 0){
                currentIndex = clothes.Count - 1;
            }

            clothes[currentIndex].SetActive(true);
        }
        
    }

}
