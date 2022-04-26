using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{

    [SerializeField] private GameObject snapPointParent;
    [SerializeField] private int nrNotesPerBeat = 13;
    [SerializeField] private GameObject marker;

    private GameObject[] snapPoints;


    // Start is called before the first frame update
    void Start()
    {
        InitializeSnapPointsArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Initializes snap points array
    private void InitializeSnapPointsArray()
    {
        // Get number of snap points
        int nrSnapPoints = snapPointParent.transform.childCount;
        snapPoints = new GameObject[nrSnapPoints];

        // Add all snap points to array
        for (int i = 0; i < nrSnapPoints; i++)
        {
            GameObject child = snapPointParent.transform.GetChild(i).gameObject;
            snapPoints[i] = child;
        }
    }

    public void PlayMusicLoop()
    {
        List<GameObject>[] noteSequence = ParseNoteData();
        for (int i = 0; i < noteSequence.Length; i++)
        {
            StartCoroutine(PlayNoteAfterTime((i + 1), noteSequence[i]));
        }
    }

    private IEnumerator PlayNoteAfterTime(float time, List<GameObject> notes)
    {
        yield return new WaitForSeconds(time);

        PlayOneNote(notes);
    }

    private void PlayOneNote(List<GameObject> notes)
    {
        foreach (GameObject note in notes)
        {
            if (note == null) continue;

            note.GetComponentInChildren<AudioSource>().Play();
        }
    }

    private List<GameObject>[] ParseNoteData()
    {
        int nrNotes = snapPoints.Length / nrNotesPerBeat;
        List<GameObject>[] noteSequence = new List<GameObject>[nrNotes];

        for (int i = 0; i < noteSequence.Length; i++)
        {
            noteSequence[i] = new List<GameObject>();
        }

        foreach (GameObject snapPoint in snapPoints)
        {
            if (snapPoint.transform.childCount < 2)
            {
                continue;
            }

            int currBarPos = ParseIntFromObjectName(snapPoint.name, "Bar");
            int currNotePos = ParseIntFromObjectName(snapPoint.name, "Note");
            
            noteSequence[4*(currBarPos - 1) + (currNotePos - 1)].Add(snapPoint);
        }
        return noteSequence;
    }

    private int ParseIntFromObjectName(string input, string splitWord)
    {
        int splitWordIndex = splitWord == "Bar" ? 0 : 1;
        return int.Parse(input.Split("-")[splitWordIndex].Split(splitWord)[1]);
    }
}
