using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropNote : MonoBehaviour
{
    private float deltaX, deltaY;
    private bool moveAllowed = false;
    private bool mouseMoveAllowed = false;
    private Vector3 originalPos;
    private Vector3 targetPosition;
    private Transform originalParent;
    private Rigidbody2D ridgidBody;

    // If the note can move
    private bool lockedInPlace = false;

    // Audio source for playing the note sound
    private AudioSource audioSource;

    // Which parent to look for snap points in
    [SerializeField] private GameObject snapPointsParent;
    private GameObject[] snapPoints;

    // Which instrument the note is
    [SerializeField] Instrument instrument;

    // Every note for all instrumets
    [SerializeField] private AudioClip[] pianoClips;
    [SerializeField] private AudioClip[] ukeleleClips;
    [SerializeField] private AudioClip[] tromboneClips;

    void Start()
    {
        // Setup the original position
        originalPos = transform.position;
        targetPosition = originalPos;
        originalParent = transform.parent;

        // Initialize array with snap points
        if (snapPointsParent != null)
        {
            InitializeSnapPointsArray();
        }
        
        ridgidBody = GetComponent<Rigidbody2D>();

        // Gets objects audio source
        audioSource = GetComponent<AudioSource>();

        // Dissables collision between note objects 
        Physics2D.IgnoreLayerCollision(6, 6); // Notes needs to be on layer 6

    }

    // Update is called once per frame
    void Update()
    {
        // If there were any touches on the screen
        if (Input.touchCount > 0 && !lockedInPlace)
        {
            Touch touch = Input.GetTouch(0);

            // Get the position of the touch
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);


            switch (touch.phase)
            {
                // At the moment the screen was touched, look if the touch is on the object.
                // If the touch was on the object, set moveAllowed to true
                case TouchPhase.Began:
                    if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                    {
                        deltaX = touchPos.x - transform.position.x;
                        deltaY = touchPos.y - transform.position.y;
                        moveAllowed = true;
                        GetComponent<CircleCollider2D>().sharedMaterial = null;
                    }
                    break;

                // While the touch is moving and is allowed to move, update the object position
                // to the touchs position
                case TouchPhase.Moved:
                    if (moveAllowed)
                    {
                        transform.position = (new Vector3(touchPos.x - deltaX, touchPos.y - deltaY));
                    }
                    break;

                // If the touch ended(player let go off the screen), set moveAllowed to false
                case TouchPhase.Ended:
                    // Only move object if it should be moved
                    if (moveAllowed)
                    {
                        SnapToPoint(touchPos);
                    }
                    moveAllowed = false;
                    break;

            }
        }

        // For mouse controls
        else if (!lockedInPlace)
        {
            // Get the mouse position
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // If the mouse is pressed down and the mouse is over the object, set mouseMoveAllowed to true
            if (Input.GetMouseButtonDown(0) && GetComponent<Collider2D>() == Physics2D.OverlapPoint(mousePosition))
            {
                mouseMoveAllowed = true;
            }

            // If mouseMoveAllowed is true, set the object to follow the mouse until the mouse button is released
            if (mouseMoveAllowed)
            {
                transform.position = mousePosition;
                if (Input.GetMouseButtonUp(0))
                {
                    SnapToPoint(mousePosition);
                    mouseMoveAllowed = false;
                }
            }

        }

        AddForceToRidgidbody();
    }


    // Method for snapping to object to a point close to it
    private void SnapToPoint(Vector2 position)
    {
        // If the snap point array is empty don't run this method
        if (snapPoints.Length == 0) return; 

        bool snapped = false;
        
        // Start values for shortest snap point
        GameObject shortestSnapPoint = snapPoints[0];
        float shortestSnapPointDistance = float.MaxValue;

        // Loops through all snap points and keeps track of the snap point closest to the object.
        foreach (GameObject snapPoint in snapPoints)
        {
            Vector2 snapPos = snapPoint.transform.position;
            // Calculates the distance from the object to each snap point
            float distToSnapPos = Vector2.Distance(transform.position, snapPoint.transform.position);
            
            // If we found a snappoint with smaller distance update shortest snap point
            if (distToSnapPos < shortestSnapPointDistance)
            {
                shortestSnapPoint = snapPoint;
                shortestSnapPointDistance = distToSnapPos;
            }

        }

        // If the object is close to the snapPoint, set the objects position to that point
        if (shortestSnapPointDistance < 0.5f && !snapped)
        {
            targetPosition = shortestSnapPoint.transform.position;
            transform.SetParent(shortestSnapPoint.transform);
            
            
            snapped = true;

            // Sets the correct audio clip depending on which note the object was placed on
            SetCorrectNote(shortestSnapPoint);

            // Temporarily play note E3
            audioSource.Play();
        }


        // If we did not snap to anything, return the object to the original position
        if (!snapped)
        {
            transform.SetParent(originalParent);
            transform.position = position; // This is to have the right coordinates
            targetPosition = originalPos;
        }

    }

    // Adds force to the player ridgidbody to move towards the target point
    private void AddForceToRidgidbody()
    {
        if (!mouseMoveAllowed && !moveAllowed)
        {
            Vector2 forceVector = (targetPosition - transform.position) * 15 * (Time.deltaTime * 1000);
            ridgidBody.AddForce(forceVector);
        }
    }

    // Initializes snap points array
    private void InitializeSnapPointsArray()
    {
        // Get number of snap points
        int nrSnapPoints = snapPointsParent.transform.childCount;
        snapPoints = new GameObject[nrSnapPoints];

        // Add all snap points to array
        for (int i = 0; i < nrSnapPoints; i++)
        {
            GameObject child = snapPointsParent.transform.GetChild(i).gameObject;
            snapPoints[i] = child;
        }
    }

    // Changes audio clip source to match the current note the object is attached to 
    private void SetCorrectNote(GameObject snapPoint)
    {
        // Get the note portion of the objects name
        string snapPointNote = snapPoint.name.Split("-")[2];

        // Get the correct audio clip based on which note to play
        AudioClip audioClip = GetAudioClip(snapPointNote);

        // If a clip is found update audio source with new clip
        if (audioClip != null) audioSource.clip = audioClip;
    }

    // Gets correct note based on which instrument the object is
    private AudioClip GetAudioClip(string noteName)
    {
        return instrument switch
        {
            Instrument.Piano => FindAudioClipNote(pianoClips, noteName),
            Instrument.Ukulele => FindAudioClipNote(ukeleleClips, noteName),
            Instrument.Trumpet => FindAudioClipNote(tromboneClips, noteName),
            _ => null,
        };
    }

    // Finds a specific note in an audio clip array
    private AudioClip FindAudioClipNote(AudioClip[] clips, string noteName)
    {
        foreach (AudioClip clip in clips)
        {
            // If we found a note with matching name, return it
            if (clip.name == noteName) return clip;
        }
        return null;
    }

    // Locks the note so it can't be moved
    public void LockNode()
    {
        lockedInPlace = true;
    }

    // Unlocks the note so it can be moved
    public void UnlockNode()
    {
        lockedInPlace = false;
    }

    // Sets the snap point parent object and initializes the array
    public void SetSnapPointParent(GameObject parent)
    {
        snapPointsParent = parent;
        InitializeSnapPointsArray();
    }
}
