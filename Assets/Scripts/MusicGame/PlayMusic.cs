using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{

    [SerializeField] private GameObject snapPointParent;
    [SerializeField] private int nrNotesPerBeat = 13;
    [SerializeField] private GameObject marker;

    private GameObject[] snapPoints;
    private int totalNrNotes;
    private int playedNotes = 0;
    private MarkerMover markerMoverScript;


    // Start is called before the first frame update
    void Start()
    {
        InitializeSnapPointsArray();
        markerMoverScript = GetComponent<MarkerMover>();
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

        markerMoverScript.ResetPlayer();

        StopAllCoroutines();

        StartCoroutine(PlayNoteAfterTime(1, noteSequence));
    }

    // Waits for a specified amount of time before playing the list of notes (the beat)
    private IEnumerator PlayNoteAfterTime(float time, List<GameObject>[] notes)
    {
        for (int i = 0; i < notes.Length-1; i++)
        {
            yield return new WaitForSeconds(time);
            
            PlayNotes(notes[i], i, notes.Length-1);
        }
        
    }

    // Play all notes in a beat
    private void PlayNotes(List<GameObject> notes, int index, int maxIndex)
    {
        markerMoverScript.SetNewDestination(index % maxIndex);
        markerMoverScript.TeleportPlayerToCurrentDestination(index);

        foreach (GameObject note in notes)
        {
            // Skip if the note is null
            if (note == null) continue;

            // Play the sound of the note
            note.GetComponentInChildren<AudioSource>().Play();

            // Increment counter
            playedNotes++;
        }
    }

    // Parses all note data in to an array of lists where the lists contains all notes that should be played on the same beat.
    private List<GameObject>[] ParseNoteData()
    {
        // Calculates the number of note beats in the sequence.
        int nrBeats = snapPoints.Length / nrNotesPerBeat;
        List<GameObject>[] noteSequence = new List<GameObject>[nrBeats];

        // Initalize a new list for every element in the array
        for (int i = 0; i < noteSequence.Length; i++)
        {
            noteSequence[i] = new List<GameObject>();
        }

        // Go through every snap point 
        foreach (GameObject snapPoint in snapPoints)
        {
            // If snap point donsn't contain a note, skip it.
            if (snapPoint.transform.childCount < 2)
            {
                continue;
            }

            // Get the current position of the note
            int currBarPos = ParseIntFromObjectName(snapPoint.name, "Bar");
            int currNotePos = ParseIntFromObjectName(snapPoint.name, "Note");
            
            // Add the snap point to the current beat in the note sequence
            noteSequence[4*(currBarPos - 1) + (currNotePos - 1)].Add(snapPoint);

            // Increment max number of notes
            totalNrNotes++;
        }
        return noteSequence;
    }

    // Parses int from specialized note name
    // Ex f("Bar1-Note2-C3", "Bar") parses to 1
    private int ParseIntFromObjectName(string input, string splitWord)
    {
        // Choose to parse bar or note value
        int splitWordIndex = splitWord == "Bar" ? 0 : 1;

        // Parses the int from string
        return int.Parse(input.Split("-")[splitWordIndex].Split(splitWord)[1]);
    }
}
