using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{

    [SerializeField] private GameObject[] snapPoints;
    [SerializeField] private int nrNotesPerBeat = 13;

    private int notePos = 1;
    private int barPos = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private List<GameObject>[] ParseNoteData()
    {
        int nrNotes = snapPoints.Length / nrNotesPerBeat;
        List<GameObject>[] noteMatrix = new List<GameObject>[nrNotes];
        foreach (GameObject snapPoint in snapPoints)
        {
            if (snapPoint.transform.childCount < 2)
            {
                continue;
            }

            int currBarPos = ParseIntFromObjectName(snapPoint.name, "Bar");
            int currNotePos = ParseIntFromObjectName(snapPoint.name, "Note");
            
            noteMatrix[4*(currBarPos - 1) + (currNotePos - 1)].Add(snapPoint);
        }
        return noteMatrix;
    }

    private int ParseIntFromObjectName(string input, string splitWord)
    {
        int splitWordIndex = splitWord == "Bar" ? 0 : 1;
        return int.Parse(input.Split("-")[splitWordIndex].Split(splitWord)[1]);
    }
}
