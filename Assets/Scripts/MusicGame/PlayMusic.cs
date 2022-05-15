using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayMusic : MonoBehaviour
{
    // Parent with all snap points
    [SerializeField] private GameObject snapPointParent;

    // Parent with all unplaced notes
    [SerializeField] private GameObject parentWithNotes;

    // Max number of notes there are per beat
    [SerializeField] private int nrNotesPerBeat = 13;

    [SerializeField] private float secondsBetweenBeats = 1;

    [SerializeField] private Button restartButton;

    [SerializeField] private Toggle playToggle;

    private GameObject[] snapPoints;

    // Is the song playing
    private bool isPlaying = false;

    // If the music should loop
    public bool isLooping { get; set; }

    // Reference to moving script for marker
    private MarkerMover markerMoverScript;

    // Start is called before the first frame update
    void Start()
    {
        InitializeSnapPointsArray();

        // Get marker mover script
        markerMoverScript = GetComponent<MarkerMover>();

        // Sync time between beats to time between jumps
        markerMoverScript.ChangeJumpTime(secondsBetweenBeats - 0.03f);
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

    // Starts to play music
    public void PlayMusicLoop()
    {
        // Prevents unwanted call when isOn is manually changed when the music stops
        if (!playToggle.isOn && !isPlaying) return;

        if (isPlaying)
        {
            // Cancel the previous run of coroutines
            StopAllCoroutines();

            // Resets music player
            ResetMusicPlayer();
        } else
        {
            // Resets marker
            markerMoverScript.ResetPlayer();

            // Get note data
            List<GameObject>[] noteSequence = ParseNoteData();

            // Cancel the previous run of coroutines
            StopAllCoroutines();

            // Start coroutine to play all notes with 1 second delay
            StartCoroutine(PlayNoteAfterTime(secondsBetweenBeats, noteSequence));

            // Sets all the variables to start playing
            PlayOrStopSetVars(true);
        }
    }

    // Waits for a specified amount of time before playing the list of notes (the beat)
    private IEnumerator PlayNoteAfterTime(float time, List<GameObject>[] notes)
    {
        bool lastRowContainNotes = ArrayContainNotes(notes, notes.Length/2);

        // Jump to the first node
        markerMoverScript.SetNewDestination(2);

        // Wait for the marker to reach the first playable node
        yield return new WaitForSeconds(time);

        do {
            bool wantToExit = false;
            int markerPosOffset = 1;

            for (int i = 0; i < notes.Length; i++) {
                int markerPos = i + markerPosOffset;

                // Sets the location to the next note to be played
                markerMoverScript.SetNewDestination((markerPos + 1) % (notes.Length + markerPosOffset));
       
                if (i == (notes.Length / 2) - 1)
                {
                    if (!lastRowContainNotes)
                    {
                        markerMoverScript.SetNewDestination(isLooping ? 1 : 0);
                        wantToExit = true;
                        
                    } else
                    {
                        markerPosOffset = 3;
                        markerMoverScript.SetNewDestination(markerPos + markerPosOffset);
                    }
                    
                } 

                // If current beat is the second last position decide to loop or not
                if (i >= notes.Length - 1)
                {
                    markerMoverScript.SetNewDestination(isLooping ? 1 : 0);
                }

                // Teleport the marker to the current note (if the jump hasn't finished)
                markerMoverScript.TeleportToDestination(markerPos);

                // Play all notes in a beat
                PlayNotes(notes[i]);

                yield return new WaitForSeconds(time);

                if (wantToExit) break;
            }
        } while (isLooping);

        // Resets music player
        ResetMusicPlayer();
    }

    // Play all notes in a beat and teleports location to supplied  
    private void PlayNotes(List<GameObject> notes)
    {
        
        // Loop through all notes in a beat
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

    // Sets button text, isPlaying and locks/unlocks all nodes based if the loop should play or stop
    private void PlayOrStopSetVars(bool play)
    {
        isPlaying = play;

        // Lock all notes
        LockOrUnlockAllNodes(!play);

        restartButton.interactable = !play;
    }

    // Make the note movable or not movable
    private void LockOrUnlockAllNodes(bool unlock)
    {
        List<DragAndDropNote> allNotes = new List<DragAndDropNote>();
        // Add all notes attached to bars
        allNotes.AddRange(snapPointParent.GetComponentsInChildren<DragAndDropNote>());

        // Add all not placed notes 
        allNotes.AddRange(parentWithNotes.GetComponentsInChildren<DragAndDropNote>());
        
        // Go through all notes
        foreach (DragAndDropNote note in allNotes)
        {
            // Unlock note if it should be unlocked
            if (unlock)
            {
                note.UnlockNode();
            } else
            {
                note.LockNode();
            }
        }
    }

    private bool ArrayContainNotes(List<GameObject>[] notes, int startIndex)
    {
        for (int i = startIndex; i < notes.Length; i++)
        {
            if (notes[i].Count > 0) return true;
        }
        return false;
    }

    // Deletes all placed notes 
    public void DeleteAllNotes()
    {
        foreach (GameObject snapPoint in snapPoints)
        {
            // If snap point donsn't contain a note, skip it.
            if (snapPoint.transform.childCount < 2) continue;

            // Only works if the loop is not playing
            if (!isPlaying)
            {
                // Gets all scripts of notes
                DragAndDropNote[] notes = snapPoint.transform.GetComponentsInChildren<DragAndDropNote>();

                // Tell all notes to unsnap and delete itself
                foreach (DragAndDropNote note in notes)
                {
                    note.SnapToOriginalPosAndDelete();
                }
            }
        }
    }

    private void ResetMusicPlayer()
    {
        // Resets marker
        markerMoverScript.ResetPlayer();

        // Sets all the variables to stop playing
        PlayOrStopSetVars(false);

        // Reset play button to original state
        playToggle.isOn = false;
    }

    public void ToggleLooping()
    {
        isLooping = !isLooping;
    }
}
